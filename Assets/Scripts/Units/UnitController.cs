using System;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Events;
using Services;
using UnityEngine;
using UnityEngine.EventSystems;


public class UnitController : MonoBehaviour
{
    [SerializeField] private int actionCountPerTurn = 0;
    [SerializeField] private GameEvent selectUnitEvent;
    [SerializeField] private GameEvent deselectUnitEvent;
    [SerializeField] private List<AttackEffect> attackEffectPrefabs;

    private PlayerUnit selectedPlayerUnit;
    private GridController gridController;
    private AbilityController abilityController;
    private Grid grid;
    /// We need to keep track of all the units that are moving or attacking because we do not want to move into the 
    /// next turn before all the units are done doing whatever
    private int actingUnitsCount;

    public List<PlayerUnit> allPlayerUnits = new List<PlayerUnit>();
    public bool IsAnyUnitActing => actingUnitsCount > 0;
    public PlayerUnit SelectedPlayerUnit => selectedPlayerUnit;
    public int ActionCountPerTurn => actionCountPerTurn;
    public Vector2Int SelectedUnitCell => (Vector2Int) grid.WorldToCell(selectedPlayerUnit.transform.position);
    public IEnumerable<PlayerUnit> AllPlayerUnits => allPlayerUnits.AsReadOnly();

    [Header("Audio stuff")]
    [SerializeField] private AudioClip damageSfx;
    [SerializeField] private AudioClip healSfx;
    [SerializeField] private AudioClip selectUnitSfx;
    [SerializeField] private AudioClip deselectUnitSfx;
    private AudioManager _audioManager;

    private void Awake()
    {
        ServiceLocator.Current.Get<UnitManager>().Controller = this;
    }

    private void Start()
    {
        gridController = ServiceLocator.Current.Get<GridManager>().Controller;
        grid = gridController.Grid;
        
        GameObject[] tempUnits;
        tempUnits = GameObject.FindGameObjectsWithTag("Unit");

        foreach (GameObject unit in tempUnits)
        {
            allPlayerUnits.Add(unit.gameObject.GetComponent<PlayerUnit>());
        }

        abilityController = ServiceLocator.Current.Get<AbilityUIManager>().AbilityController;
        _audioManager = ServiceLocator.Current.Get<AudioManager>();
    }

    private void Update()
    {
        HandleClick();
    }

    public List<PlayerUnit> GetPlayerUnits() 
    {
        return allPlayerUnits;
    }

    /// <summary>
    /// Get any unit given a position in the world, returns null otherwise if no unit is found.
    /// </summary>
    public IUnit GetUnitAt(Vector2 pos)
    {
        return GetUnitAt((Vector2Int) grid.WorldToCell(pos));
    }

    /// <summary>
    /// Get any unit given a cell, returns null otherwise if no unit is found.
    /// </summary>
    public IUnit GetUnitAt(Vector2Int cell)
    {
        Collider2D hitCollider = Physics2D.OverlapPoint(cell + (Vector2) grid.cellSize / 2f);

        if (hitCollider != null)
        {
            IUnit hitUnit = hitCollider.GetComponent<IUnit>();

            if (hitUnit != null)
            {
                return hitUnit;
            }
        }

        return null;
    }

    /// <summary>
    /// Get a player unit given a position in the world, returns null otherwise if no unit is found.
    /// </summary>
    public PlayerUnit GetPlayerUnitAt(Vector2 pos)
    {
        return GetPlayerUnitAt((Vector2Int) grid.WorldToCell(pos));
    }

    /// <summary>
    /// Get a player unit given a cell, returns null otherwise if no unit is found.
    /// </summary>
    public PlayerUnit GetPlayerUnitAt(Vector2Int cell)
    {
        foreach (PlayerUnit unit in AllPlayerUnits)
        {
            if (unit.CurrentCell == cell)
            {
                return unit;
            }
        }

        return null;
    }

    private void HandleClick()
    {
        if (!EventSystem.current)
        {
            Debug.Log("EventSystem is missing");
            return;
        }

        if (Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

            if (hitCollider != null && (hitCollider.CompareTag("Unit") || hitCollider.CompareTag("Enemy")))
            {
                IUnit hitUnit = hitCollider.GetComponent<IUnit>();

                if (hitUnit != null)
                {
                    ClickUnit(hitUnit);
                }
            }
            else
            {
                // Here we have clicked on an empty space
                MoveSelectedUnitTo(mousePosition);
            }
        }

        if (Input.GetButtonDown("Escape") && selectedPlayerUnit != null)
        {
            DeselectSelectedPlayerUnit();
        }
    }

    private void ClickUnit(IUnit unit)
    {

        Debug.Log("DING DONT BING");
        if (unit is PlayerUnit playerUnit && selectedPlayerUnit == null)
        {
            SelectPlayerUnit(playerUnit);
            _audioManager.PlaySound(selectUnitSfx);
        }
        else if ((unit is PlayerUnit || unit is EnemyUnit) && abilityController.isAbilitySelected && canCast(abilityController.curSelectedAbility.elementalCost)) // &&hitCollider.CompareTag("Enemy")
        {
            AttackSelectedUnit(unit); //  TO ATTACK THE UNIT || change mouse position to the enemy unit that is going to be damaged
        }
        else
        {
            DeselectSelectedPlayerUnit();
        }
    }

    private void SelectPlayerUnit(PlayerUnit playerUnit)
    {
        Debug.Log($"Selected {playerUnit}");

        selectedPlayerUnit = playerUnit;
        selectUnitEvent.Raise();
    }

    private void DeselectSelectedPlayerUnit()
    {
        Debug.Log($"Unselected {selectedPlayerUnit}");

        if (selectedPlayerUnit != null)
        {
            selectedPlayerUnit = null;
            _audioManager.PlaySound(deselectUnitSfx);
            deselectUnitEvent.Raise();
        }
    }

    private async void MoveSelectedUnitTo(Vector2 targetPos)
    {
        if (selectedPlayerUnit == null) return;

        actingUnitsCount++;
        // Store the variable locally because player might get unselected while this is running Asynchronously
        PlayerUnit currentUnit = selectedPlayerUnit;

        if (!abilityController.isAbilitySelected)
        {
            Debug.Log($"Move {selectedPlayerUnit} towards {targetPos}");

            Vector2Int targetCellPos = (Vector2Int) grid.WorldToCell(targetPos);
            Vector2Int selectedUnitCellPos = (Vector2Int) grid.WorldToCell(currentUnit.transform.position);

            int distance = Vector2IntUtils.ManhattanDistance(targetCellPos, selectedUnitCellPos);

            if (currentUnit.CanMove && distance <= currentUnit.CurrentMovementPoints)
            {
                // Vector2 finalPos = grid.CellToWorld(grid.WorldToCell(targetPos)) + grid.cellSize / 2f;
                // currentUnit.transform.position = finalPos;

                UnitMover unitMover = currentUnit.GetComponent<UnitMover>();
                // Make sure to deselect the unit before we apply the animations or let the player do anything else
                DeselectSelectedPlayerUnit();
                
                // Do some movement animations
                await unitMover.MoveTo(targetCellPos);
                
                currentUnit.CurrentMovementPoints -= distance;
                currentUnit.ActionCount--;
            }
            else
            {
                Debug.Log($"Can't move unit, lacking {distance - currentUnit.CurrentMovementPoints} movement points");
            }
        }

        actingUnitsCount--;
    }

    private void AttackSelectedUnit(IUnit target) // SET THE NEW PARAMETER FOR THIS METHOD TO BE THE ENEMY UNIT
    {
        Debug.Log("firstest");

        if (selectedPlayerUnit == null) return;

        Vector2Int targetCellPos = (Vector2Int) grid.WorldToCell(target.transform.position);
        Vector2Int selectedUnitCellPos = (Vector2Int) grid.WorldToCell(selectedPlayerUnit.transform.position);

        int distance = Vector2IntUtils.ManhattanDistance(targetCellPos, selectedUnitCellPos);

        AbilityStats selectedAbility = abilityController.curSelectedAbility;

        float damage = selectedAbility.damage;
        
            if (distance <= selectedAbility.range)
            {
                    if (selectedAbility.Description.Contains("+1") && target is PlayerUnit) 
                  {
                      target.transform.GetComponent<PlayerUnit>().AddToken(selectedAbility.element);
    
                  } 
                  else  
                  {

                    if (damage > 0)
                    {
                        Element attackingElement = selectedPlayerUnit.playerData.elementType;
                        damage *= attackingElement.AttackWithThisElement(target.Element);
                        damage = Mathf.Max(damage, 1);
                        target.Health.TakeDamage((int)damage);
                        
                        // Play attack animation
                        CreateAttackEffect(targetCellPos, attackingElement);
                        _audioManager.PlaySound(damageSfx);
                        
                    }
                    else
                    {
                        target.Health.TakeHeal(selectedAbility.damage);
                        _audioManager.PlaySound(healSfx);

                    }
                }

                UpdateTokens(selectedAbility.elementalCost);

            }
           
            //     {


            //     int distance = Vector2IntUtils.ManhattanDistance(targetCellPos, selectedUnitCellPos);

            //     if ( distance <= abilityController.curSelectedAbility.range) 
            //     {
            //         // TODO Maybe we want some cool coroutine or animation here later
            //         Vector2 finalPos = grid.CellToWorld(grid.WorldToCell(targetPos)) + grid.cellSize / 2f;
            //         selectedUnit.transform.position = finalPos;
            //         selectedUnit.CurrentMovementPoints -= distance;
            //         selectedUnit.ActionCount--;

            //         Debug.Log($"Move to final pos {finalPos}");
            //     }
            //     else
            //     {
            //         Debug.Log($"Can't move unit, lacking {distance - selectedUnit.CurrentMovementPoints} movement points");
            //     }

            //     // Unselect the unit at the end
                  DeselectSelectedPlayerUnit();
        }

        void UpdateTokens(List<Element> usedTokens) 
        {
            // for (int i = 0; i <= selectedUnit.playerData.currentTokens.Count - 1; i++) 
            // {
            //    foreach(Element token in usedTokens) 
            //    {
            //            if (token == selectedUnit.playerData.currentTokens[i]) 
            //        {
            //            selectedUnit.playerData.currentTokens.RemoveAt(i);
            //        }
            //    }
            // }

            foreach(Element token in usedTokens) 
            {
                for (int i = 0; i <= selectedPlayerUnit.playerData.currentTokens.Count -1; i++) 
                {
                    if (token == selectedPlayerUnit.playerData.currentTokens[i]) 
                    {
                        selectedPlayerUnit.playerData.currentTokens.RemoveAt(i);
                        break;
                    }
                }
            }

            


        }

        bool canCast(List<Element> usedTokens) 
        {

            int a = 0;
               for (int i = 0; i <= selectedPlayerUnit.playerData.currentTokens.Count -1; i++) 
            {
               foreach(Element token in usedTokens) 
               {
                   if (token == selectedPlayerUnit.playerData.currentTokens[i]) 
                   {
                       a++;
                       break;
                       
                   }
               }
            }

            if (a >= usedTokens.Count) 
            {
                return true;
            }

            return false;
        }

    public void CreateAttackEffect(Vector2Int cell, Element element)
    {
        AttackEffect attackEffect = null;

        foreach (AttackEffect attackEffectPrefab in attackEffectPrefabs)
        {
            if (attackEffectPrefab.Element == element)
            {
                attackEffect = Instantiate(attackEffectPrefab);
            }
        }

        if (attackEffect == null)
        {
            Debug.LogError($"Found unregistered element {element} that has no associate attack effect!");
            return;
        }
         
        attackEffect.SetAtCell(cell);
    }
}