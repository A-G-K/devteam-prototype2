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

    [SerializeField] private Tilemap tilemap;

    private Grid grid;
    private UnitController unitController;
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
        grid = unitController.Grid;

        abilityController = ServiceLocator.Current.Get<AbilityUIManager>().AbilityController;

        validTile = ScriptableObject.CreateInstance<Tile>();
        validTile.sprite = validCellSprite;

        selectedTile = ScriptableObject.CreateInstance<Tile>();
        selectedTile.sprite = selectedCellSprite;

          validAbilityCell = ScriptableObject.CreateInstance<Tile>();
          validAbilityCell.sprite = abilityCellSprite;
    }

    public void OnUnitSelected()
    {
        tilemap.SetTile((Vector3Int) unitController.SelectedUnitCell, selectedTile);

        if (unitController.SelectedUnit.CanMove)
        {
            HighlightValidTiles(unitController.SelectedUnit.CurrentMovementPoints, validTile);
        }
    }

    public void OnUnitDeselected()
    {
        tilemap.ClearAllTiles();
    }

    public void HighlightAbilityRange(int range) 
    {
        tilemap.ClearAllTiles();
        HighlightValidTiles(range, validAbilityCell);
    } 


    
    private void HighlightValidTiles(int range, Tile tileType)
    {
        foreach (Vector2Int cell in 
            // Vector2IntUtils.GetNearbyCells(unitController.SelectedUnitCell, unitController.SelectedUnit.CurrentMovementPoints))
            Vector2IntUtils.GetNearbyCells(unitController.SelectedUnitCell, range))

        {
            tilemap.SetTile((Vector3Int) cell, tileType);
        }
    }
}