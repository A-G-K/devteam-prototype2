using System;
using UnityEngine;


public class UnitController : MonoBehaviour
{
    [SerializeField] private Grid grid;
    
    private Unit selectedUnit;
    
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
            Debug.Log($"Selected {unit}");
            
            // Here we select a unit
            selectedUnit = unit;
        }
        else
        {
            Debug.Log($"Unselected {selectedUnit}");
            
            // Here we un-select a unit
            selectedUnit = null;
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
        selectedUnit = null;
    }
}