using System;
using UnityEngine;


public class AttackEffect : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Element element;

    private GridController gridController;
    private Grid Grid => gridController.Grid;
    public Element Element => element;

    private void Start()
    {
        gridController = ServiceLocator.Current.Get<GridManager>().Controller;
    }

    public void SetAtCell(Vector2Int cell)
    {
        transform.position = Grid.CellToWorld((Vector3Int) cell) + Grid.cellSize / 2f;
    }

    private void Update()
    {
        // TODO destroy the gameobject when animator is done somehow?
    }
}