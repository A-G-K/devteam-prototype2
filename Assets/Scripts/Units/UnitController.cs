using System;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Events;
using Services;
using UnityEngine;
using UnityEngine.EventSystems;


public class UnitController : MonoBehaviour
{
    [SerializeField] private int actionCountPerTurn = 1;
    [SerializeField] private GameEvent selectUnitEvent;
    [SerializeField] private GameEvent deselectUnitEvent;

    private PlayerUnit selectedPlayerUnit;
    private GridController gridController;
    private Grid grid;

    public List<PlayerUnit> allPlayerUnits = new List<PlayerUnit>();
    public PlayerUnit SelectedPlayerUnit => selectedPlayerUnit;
    public int ActionCountPerTurn => actionCountPerTurn;
    public Vector2Int SelectedUnitCell => (Vector2Int) grid.WorldToCell(selectedPlayerUnit.transform.position);
    public IEnumerable<PlayerUnit> AllPlayerUnits => allPlayerUnits.AsReadOnly();

    private AbilityController abilityController;

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

    /// <summary>
    /// Get a player unit given a position in the world, returns null otherwise if no unit is found.
    /// </summary>
    public PlayerUnit GetUnitAt(Vector2 pos)
    {
        return GetUnitAt((Vector2Int) grid.WorldToCell(pos));
    }

    /// <summary>
    /// Get a player unit given a cell, returns null otherwise if no unit is found.
    /// </summary>
    public PlayerUnit GetUnitAt(Vector2Int cell)
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

            if (hitCollider != null && hitCollider.CompareTag("Unit"))
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
        if (unit is PlayerUnit playerUnit && selectedPlayerUnit == null)
        {
            SelectPlayerUnit(playerUnit);
            _audioManager.PlaySound(selectUnitSfx);
        }
        else if (unit is EnemyUnit && abilityController.isAbilitySelected && canCast(abilityController.curSelectedAbility.elementalCost)) // &&hitCollider.CompareTag("Enemy")
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

        if (!abilityController.isAbilitySelected)
        {
            Debug.Log($"Move {selectedPlayerUnit} towards {targetPos}");

            Vector2Int targetCellPos = (Vector2Int) grid.WorldToCell(targetPos);
            Vector2Int selectedUnitCellPos = (Vector2Int) grid.WorldToCell(selectedPlayerUnit.transform.position);

            int distance = Vector2IntUtils.ManhattanDistance(targetCellPos, selectedUnitCellPos);

            if (selectedPlayerUnit.CanMove && distance <= selectedPlayerUnit.CurrentMovementPoints)
            {
                // TODO Maybe we want some cool coroutine or animation here later
                Vector2 finalPos = grid.CellToWorld(grid.WorldToCell(targetPos)) + grid.cellSize / 2f;
                selectedPlayerUnit.transform.position = finalPos;
                selectedPlayerUnit.CurrentMovementPoints -= distance;
                selectedPlayerUnit.ActionCount--;

                Debug.Log($"Move to final pos {finalPos}");
            }
            else
            {
                Debug.Log($"Can't move unit, lacking {distance - selectedPlayerUnit.CurrentMovementPoints} movement points");
            }

            // Unselect the unit at the end
            DeselectSelectedPlayerUnit();
        }
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
                if (damage > 0)
                {
                    damage *= selectedPlayerUnit.playerData.elementType.AttackWithThisElement(target.Element);
                    damage = Mathf.Max(damage, 1);
                    target.Health.TakeDamage((int)damage);
                    
                    _audioManager.PlaySound(damageSfx);
                    
                    UpdateTokens(selectedAbility.elementalCost);
                }
                else
                {
                    Debug.Log("cheff2");
                    target.Health.TakeHeal(selectedAbility.damage);
                    _audioManager.PlaySound(healSfx);
                    UpdateTokens(selectedAbility.elementalCost);

                }
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
            for (int i = 0; i < selectedPlayerUnit.playerData.currentTokens.Count - 1; i++) 
            {
               foreach(Element token in usedTokens) 
               {
                   if (token == selectedPlayerUnit.playerData.currentTokens[i]) 
                   {
                       selectedPlayerUnit.playerData.currentTokens.RemoveAt(i);
                       
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
}