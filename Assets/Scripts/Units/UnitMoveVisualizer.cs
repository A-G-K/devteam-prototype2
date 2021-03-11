using System;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vector2IntExtension;


public class UnitMoveVisualizer : MonoBehaviour
{
    [SerializeField] private TileBase validTile;
    [SerializeField] private TileBase selectedTile;
    [SerializeField] private TileBase abilityTile;
    
    private Tilemap visualTilemap;
    private Grid grid;
    private UnitController unitController;
    private GridController gridController;
    private AbilityController abilityController;
    private UnitManager unitManager;

    private void Awake()
    {
        ServiceLocator.Current.Get<UnitManager>().Visualizer = this;
    }
    
    private void Start()
    {
        unitController = ServiceLocator.Current.Get<UnitManager>().Controller;
        gridController = ServiceLocator.Current.Get<GridManager>().Controller;
        grid = gridController.Grid;
        abilityController = ServiceLocator.Current.Get<AbilityUIManager>().AbilityController;

        visualTilemap = gridController.VisualTilemap;
    }

    public void OnUnitSelected()
    {
        visualTilemap.SetTile((Vector3Int) unitController.SelectedUnitCell, selectedTile);

        if (unitController.SelectedUnit.CanMove)
        {
            HighlightValidTiles(unitController.SelectedUnit.CurrentMovementPoints, validTile);
        }
    }

    public void OnUnitDeselected()
    {
        visualTilemap.ClearAllTiles();
    }

    public void HighlightAbilityRange(int range) 
    {
        visualTilemap.ClearAllTiles();
        HighlightValidTiles(range, abilityTile);
    } 


    
    private void HighlightValidTiles(int range, TileBase tileType)
    {
        foreach (Vector2Int cell in 
            // Vector2IntUtils.GetNearbyCells(unitController.SelectedUnitCell, unitController.SelectedUnit.CurrentMovementPoints))
            Vector2IntUtils.GetNearbyCells(unitController.SelectedUnitCell, range))

        {
            visualTilemap.SetTile((Vector3Int) cell, tileType);
        }
    }
}