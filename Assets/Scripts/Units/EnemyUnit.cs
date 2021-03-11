using System;
using UnityEngine;


[RequireComponent(typeof(Health))]
public class EnemyUnit : MonoBehaviour, IUnit
{
    [SerializeField] private Element element;

    private GridController gridController;
    
    public Health Health { get; private set; }
    public Element Element => element;
    public Vector2Int CurrentCell => (Vector2Int) gridController.Grid.WorldToCell(transform.position);

    private void Awake()
    {
        Health = GetComponent<Health>();
    }

    private void Start()
    {
        gridController = ServiceLocator.Current.Get<GridManager>().Controller;
    }
}