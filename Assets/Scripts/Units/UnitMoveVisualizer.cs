using System;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vector2IntExtension;


public class UnitMoveVisualizer : MonoBehaviour
{
    [SerializeField] private Sprite validCellSprite;
    [SerializeField] private Sprite selectedCellSprite;
    [SerializeField] private Sprite abilityCellSprite;
    
    private Tilemap visualTilemap;
    private Grid grid;
    private UnitController unitController;
    private GridController gridController;
    private Tile validTile;
    private Tile selectedTile;
    private Tile validAbilityCell;
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

        validTile = ScriptableObject.CreateInstance<Tile>();
        validTile.sprite = validCellSprite;

        selectedTile = ScriptableObject.CreateInstance<Tile>();
        selectedTile.sprite = selectedCellSprite;

        validAbilityCell = ScriptableObject.CreateInstance<Tile>();
        validAbilityCell.sprite = abilityCellSprite;
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
        HighlightValidTiles(range, validAbilityCell);
    } 


    
    private void HighlightValidTiles(int range, Tile tileType)
    {
        foreach (Vector2Int cell in 
            // Vector2IntUtils.GetNearbyCells(unitController.SelectedUnitCell, unitController.SelectedUnit.CurrentMovementPoints))
            Vector2IntUtils.GetNearbyCells(unitController.SelectedUnitCell, range))

        {
            visualTilemap.SetTile((Vector3Int) cell, tileType);
        }
    }
}