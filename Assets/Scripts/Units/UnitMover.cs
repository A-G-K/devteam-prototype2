using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;


public class UnitMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float footHeightOffset = 0.1f;
    [SerializeField] private Animator movementAnimator;

    private GridController gridController;
    private static readonly int IsMovingParam = Animator.StringToHash("IsMoving");
    private static readonly int IsMovingLeftParam = Animator.StringToHash("IsMovingLeft");

    public Vector2Int CurrentCell => (Vector2Int) gridController.Grid.WorldToCell(transform.position);
    public Grid Grid => gridController.Grid;
    
    private void Start()
    {
        gridController = ServiceLocator.Current.Get<GridManager>().Controller;
        SnapToCurrentCell();
    }
    
    public void SnapToCurrentCell()
    {
        InstantlyMoveTo(CurrentCell);
    }

    public void InstantlyMoveTo(Vector2Int targetCell)
    {
        Vector2 finalPos = gridController.Grid.CellToWorld((Vector3Int) targetCell) + gridController.Grid.cellSize / 2f;
        finalPos.y += footHeightOffset;
        transform.position = finalPos;
    }

    public async Task MoveTo(Vector2Int targetCell)
    {
        Navigator navigator = new Navigator(Grid, CurrentCell);
        List<Vector2Int> path = navigator.CalculateNavigationCells(targetCell);
        await MoveAlongPath(path);
    }

    private async Task MoveAlongPath(IEnumerable<Vector2Int> navigationCells)
    {
        movementAnimator.SetBool(IsMovingParam, true);
        
        foreach (Vector2Int targetCell in navigationCells.Skip(1))
        {
            // Here we convert a given cell into a position
            Vector2 targetPos = targetCell + (Vector2) Grid.cellSize / 2f;
            targetPos.y += footHeightOffset;
            
            float lastMoveSpeed = moveSpeed;
            Vector2 currentPosition = transform.localPosition;
            float distance = Vector2.Distance(currentPosition, targetPos);
            float timeTaken = distance / (moveSpeed * Time.fixedDeltaTime);
            float timeIncrement = 1f / timeTaken;

            // Animation
            bool isMovingLeft = (targetPos - currentPosition).x < 0;
            movementAnimator.SetBool(IsMovingLeftParam, isMovingLeft);

            for (float i = 0; i < 1; i += timeIncrement)
            {
                transform.localPosition = Vector2.Lerp(currentPosition, targetPos, i);
                await UniTask.Yield();
                
                if (lastMoveSpeed != moveSpeed)
                {
                    timeTaken = distance / (moveSpeed * Time.fixedDeltaTime);
                    timeIncrement = 1f / timeTaken;
                    lastMoveSpeed = moveSpeed;
                }
            }
            
            transform.localPosition = targetPos;
        }
        
        movementAnimator.SetBool(IsMovingParam, false);
    }
}