using System;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Events;
using Services;
using UnityEngine;


public class UnitController : MonoBehaviour
{
    [SerializeField] private Grid grid;

    [SerializeField] private GameEvent selectUnitEvent;
    [SerializeField] private GameEvent deselectUnitEvent;
    
    private Unit selectedUnit;
    private AbilityUIManager _abilityUIManager;

    public Grid Grid => grid;

    public List<Unit> allPlayerUnits = new List<Unit>();
    public Unit SelectedUnit => selectedUnit;
    public Vector2Int SelectedUnitCell => (Vector2Int) grid.WorldToCell(selectedUnit.transform.position);

    private void Awake()
    {
        ServiceLocator.Current.Get<UnitManager>().Controller = this;
    }

    private void Start()
    {
        _abilityUIManager = ServiceLocator.Current.Get<AbilityUIManager>();
        GameObject[] tempUnits;
        tempUnits = GameObject.FindGameObjectsWithTag("Unit");

        foreach (GameObject unit in tempUnits) 
        {
           
            allPlayerUnits.Add(unit.gameObject.GetComponent<Unit>());
        }
        Debug.Log(allPlayerUnits.Count);
    }

    private void Update()
    {
        HandleClick();
    }

    private void HandleClick()
    {
        if (Input.GetButtonDown("Fire1"))
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
    }

    private void ClickUnit(Unit unit)
    {
        if (selectedUnit == null)
        {
            SelectUnit(unit);
        }
        else
        {
            DeselectSelectedUnit();
        }
    }

    private void SelectUnit(Unit unit)
    {
        Debug.Log($"Selected {unit}");
        
        selectedUnit = unit;
        _abilityUIManager.SelectedUnit = unit;
        selectUnitEvent.Raise();
    }

    private void DeselectSelectedUnit()
    {
        Debug.Log($"Unselected {selectedUnit}");
        
        if (selectedUnit != null)
        {
            selectedUnit = null;
            _abilityUIManager.SelectedUnit = null;
            deselectUnitEvent.Raise();
        }
    }

    private void MoveSelectedUnitTo(Vector2 targetPos)
    {
        if (selectedUnit == null) return;
        
        Debug.Log($"Move {selectedUnit} towards {targetPos}");

        Vector2Int targetCellPos = (Vector2Int) grid.WorldToCell(targetPos);
        Vector2Int selectedUnitCellPos = (Vector2Int) grid.WorldToCell(selectedUnit.transform.position);

        int distance = Vector2IntUtils.ManhattanDistance(targetCellPos, selectedUnitCellPos);

        if (distance <= selectedUnit.CurrentMovementPoints)
        {
            // TODO Maybe we want some cool coroutine or animation here later
            Vector2 finalPos = grid.CellToWorld(grid.WorldToCell(targetPos)) + grid.cellSize / 2f;
            selectedUnit.transform.position = finalPos;
            
            
            selectedUnit.CurrentMovementPoints -= distance;

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