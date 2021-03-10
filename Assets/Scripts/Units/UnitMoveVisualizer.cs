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
    [SerializeField] private Tilemap tilemap;

    private Grid grid;
    private UnitController unitController;
    private Tile validTile;
    private Tile selectedTile;

    private void Start()
    {
        unitController = ServiceLocator.Current.Get<UnitManager>().Controller;
        grid = unitController.Grid;

        validTile = ScriptableObject.CreateInstance<Tile>();
        validTile.sprite = validCellSprite;

        selectedTile = ScriptableObject.CreateInstance<Tile>();
        selectedTile.sprite = selectedCellSprite;
    }

    public void OnUnitSelected()
    {
        tilemap.SetTile((Vector3Int) unitController.SelectedUnitCell, selectedTile);

        if (unitController.SelectedUnit.CanMove)
        {
            HighlightValidTiles();
        }
    }

    public void OnUnitDeselected()
    {
        tilemap.ClearAllTiles();
    }
    
    private void HighlightValidTiles()
    {
        foreach (Vector2Int cell in 
            Vector2IntUtils.GetNearbyCells(unitController.SelectedUnitCell, unitController.SelectedUnit.CurrentMovementPoints))
        {
            tilemap.SetTile((Vector3Int) cell, validTile);
        }
    }
}