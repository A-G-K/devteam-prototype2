using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Services;
using UnityEngine;
using Vector2IntExtension;


[RequireComponent(typeof(UnitMover))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private int movementPoints = 3;

    private UnitController unitController;
    private GridController gridController;
    private UnitMover unitMover;
    
    public int CurrentMovementPoints { get; set; }
    public Vector2Int CurrentCell => (Vector2Int) gridController.Grid.WorldToCell(transform.position);

    private void Awake()
    {
        ResetMovement();
    }

    private void Start()
    {
        unitController = ServiceLocator.Current.Get<UnitManager>().Controller;
        gridController = ServiceLocator.Current.Get<GridManager>().Controller;
        unitMover = GetComponent<UnitMover>();
    }

    public void ResetMovement()
    {
        CurrentMovementPoints = movementPoints;
    }

    public async Task MoveTo(Vector2Int targetCell)
    {
        await unitMover.MoveTo(targetCell);
    }

    public Vector2Int TowardsNearestPlayerUnit()
    {
        IEnumerable<PlayerUnit> units = unitController.AllPlayerUnits;
        Vector2Int currentCell = CurrentCell;
        PlayerUnit nearestPlayerUnit = units.Aggregate((leftUnit, rightUnit) => 
            Vector2IntUtils.ManhattanDistance(currentCell, leftUnit.CurrentCell) < Vector2IntUtils.ManhattanDistance(currentCell, rightUnit.CurrentCell)
                ? leftUnit : rightUnit);
        
        return FindClosestCellTowards(nearestPlayerUnit.CurrentCell);
    }

    // Example of another function in this class....
    // public Vector2Int AwayFromNearestPlayerUnit()
    // {
    //     ...
    // }

    public IEnumerable<PlayerUnit> GetNearbyPlayerUnits()
    {
        List<PlayerUnit> nearbyUnits = new List<PlayerUnit>(4);
        
        foreach (Vector2Int neighbourCell in CurrentCell.GetNeighbouring())
        {
            PlayerUnit playerUnit = unitController.GetPlayerUnitAt(neighbourCell);

            if (playerUnit != null)
            {
                nearbyUnits.Add(playerUnit);
            }
        }

        return nearbyUnits;
    }

    private Vector2Int FindClosestCellTowards(Vector2Int targetCell)
    {
        Navigator navigator = new Navigator(gridController.Grid, CurrentCell);
        navigator.SetMovementValidation((fromCell, toCell) => 
            toCell == targetCell || unitController.GetUnitAt(toCell) == null);

        List<Vector2Int> navigationCells = navigator.CalculateNavigationCells(targetCell);

        // Check if there is a valid path
        if (navigationCells.Count > 2)
        {
            return navigationCells.Skip(1).Take(CurrentMovementPoints).Last(c => c != targetCell);
        }

        return CurrentCell;
    }
}