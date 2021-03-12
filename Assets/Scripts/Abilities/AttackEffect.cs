using System;
using UnityEngine;


public class AttackEffect : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Element element;

    private GridController gridController;
    private Grid Grid => gridController.Grid;
    public Element Element => element;
    
    private void Awake()
    {
        // This object is expected to be instantiated, we can get away with doing this in terms of script order execution
        gridController = ServiceLocator.Current.Get<GridManager>().Controller;
    }

    public void SetAtCell(Vector2Int cell)
    {
        transform.position = Grid.CellToWorld((Vector3Int) cell) + Grid.cellSize / 2f;
    }

    private void Update()
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.normalizedTime > stateInfo.length)
        {
            Destroy(gameObject);
        }
    }
}