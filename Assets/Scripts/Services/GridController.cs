using System;
using UnityEngine;
using UnityEngine.Tilemaps;


[RequireComponent(typeof(Grid))]
public class GridController : MonoBehaviour
{
    [SerializeField] private Tilemap mainTilemap;
    [SerializeField] private Tilemap visualTilemap;
    
    public Grid Grid { get; private set; }
    public Tilemap MainTilemap => mainTilemap;
    public Tilemap VisualTilemap => visualTilemap;

    private void Awake()
    {
        Grid = GetComponent<Grid>();
        ServiceLocator.Current.Get<GridManager>().Controller = this;
    }
}