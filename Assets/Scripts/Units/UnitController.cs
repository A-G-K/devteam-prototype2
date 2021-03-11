using System;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Events;
using Services;
using UnityEngine;
using UnityEngine.EventSystems;


public class UnitController : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private int actionCountPerTurn = 2;
    [SerializeField] private GameEvent selectUnitEvent;
    [SerializeField] private GameEvent deselectUnitEvent;

    private Unit selectedUnit;

    public Grid Grid => grid;

    public List<Unit> allPlayerUnits = new List<Unit>();
    public Unit SelectedUnit => selectedUnit;
    public int ActionCountPerTurn => actionCountPerTurn;
    public Vector2Int SelectedUnitCell => (Vector2Int) grid.WorldToCell(selectedUnit.transform.position);
    public IEnumerable<Unit> AllPlayerUnits => allPlayerUnits.AsReadOnly();

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
        GameObject[] tempUnits;
        tempUnits = GameObject.FindGameObjectsWithTag("Unit");

        foreach (GameObject unit in tempUnits)
        {
            allPlayerUnits.Add(unit.gameObject.GetComponent<Unit>());
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
    public Unit GetUnitAt(Vector2 pos)
    {
        return GetUnitAt((Vector2Int) grid.WorldToCell(pos));
    }

    /// <summary>
    /// Get a player unit given a cell, returns null otherwise if no unit is found.
    /// </summary>
    public Unit GetUnitAt(Vector2Int cell)
    {
        foreach (Unit unit in AllPlayerUnits)
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
                Unit hitUnit = hitCollider.GetComponent<Unit>();

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

        if (Input.GetButtonDown("Escape") && selectedUnit != null)
        {
            DeselectSelectedUnit();
        }
    }

    private void ClickUnit(Unit unit)
    {
        if (selectedUnit == null)
        {
            SelectUnit(unit);
            _audioManager.PlaySound(selectUnitSfx);
        }
        else if (abilityController.isAbilitySelected && canCast(abilityController.curSelectedAbility.elementalCost)) // &&hitCollider.CompareTag("Enemy")
        {
            AttackSelectedUnit(unit); //  TO ATTACK THE UNIT || change mouse position to the enemy unit that is going to be damaged
        }else
        {
            DeselectSelectedUnit();
        }
    }

    private void SelectUnit(Unit unit)
    {
        Debug.Log($"Selected {unit}");

        selectedUnit = unit;
        selectUnitEvent.Raise();
    }

    private void DeselectSelectedUnit()
    {
        Debug.Log($"Unselected {selectedUnit}");

        if (selectedUnit != null)
        {
            selectedUnit = null;
            _audioManager.PlaySound(deselectUnitSfx);
            deselectUnitEvent.Raise();
        }
    }

    private void MoveSelectedUnitTo(Vector2 targetPos)
    {
        if (selectedUnit == null) return;

        if (!abilityController.isAbilitySelected)
        {
            Debug.Log($"Move {selectedUnit} towards {targetPos}");

            Vector2Int targetCellPos = (Vector2Int) grid.WorldToCell(targetPos);
            Vector2Int selectedUnitCellPos = (Vector2Int) grid.WorldToCell(selectedUnit.transform.position);

            int distance = Vector2IntUtils.ManhattanDistance(targetCellPos, selectedUnitCellPos);

            if (selectedUnit.CanMove && distance <= selectedUnit.CurrentMovementPoints)
            {
                // TODO Maybe we want some cool coroutine or animation here later
                Vector2 finalPos = grid.CellToWorld(grid.WorldToCell(targetPos)) + grid.cellSize / 2f;
                selectedUnit.transform.position = finalPos;
                selectedUnit.CurrentMovementPoints -= distance;
                selectedUnit.ActionCount--;

                Debug.Log($"Move to final pos {finalPos}");
            }
            else
            {
                Debug.Log($"Can't move unit, lacking {distance - selectedUnit.CurrentMovementPoints} movement points");
            }

            // Unselect the unit at the end
            DeselectSelectedUnit();
        }
    }

    private void AttackSelectedUnit(Unit target) // SET THE NEW PARAMETER FOR THIS METHOD TO BE THE ENEMY UNIT
    {
        Debug.Log("firstest");

        if (selectedUnit == null) return;

        Vector2Int targetCellPos = (Vector2Int) grid.WorldToCell(target.transform.position);
        Vector2Int selectedUnitCellPos = (Vector2Int) grid.WorldToCell(selectedUnit.transform.position);

        int distance = Vector2IntUtils.ManhattanDistance(targetCellPos, selectedUnitCellPos);

        AbilityStats selectedAbility = abilityController.curSelectedAbility;

        float damage = selectedAbility.damage;
        
            if (distance <= selectedAbility.range)
            {
                if (damage > 0)
                {
                    damage *= selectedUnit.playerData.elementType.AttackWithThisElement(target.playerData.elementType);
                    damage = Mathf.Max(damage, 1);
                    target.GetComponent<Health>().TakeDamage((int)damage);
                    
                    _audioManager.PlaySound(damageSfx);
                    
                    UpdateTokens(selectedAbility.elementalCost);
                }
                else
                {
                    Debug.Log("cheff2");
                    target.GetComponent<Health>().TakeHeal(selectedAbility.damage);
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
                  DeselectSelectedUnit();
        }

        void UpdateTokens(List<Element> usedTokens) 
        {
            for (int i = 0; i < selectedUnit.playerData.currentTokens.Count - 1; i++) 
            {
               foreach(Element token in usedTokens) 
               {
                   if (token == selectedUnit.playerData.currentTokens[i]) 
                   {
                       selectedUnit.playerData.currentTokens.RemoveAt(i);
                       
                   }
               }
            }


        }

        bool canCast(List<Element> usedTokens) 
        {

            int a = 0;
               for (int i = 0; i <= selectedUnit.playerData.currentTokens.Count -1; i++) 
            {
               foreach(Element token in usedTokens) 
               {
                   if (token == selectedUnit.playerData.currentTokens[i]) 
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