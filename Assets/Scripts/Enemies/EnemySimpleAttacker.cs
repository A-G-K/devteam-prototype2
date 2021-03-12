using System;
using Services;
using UnityEngine;


public class EnemySimpleAttacker : MonoBehaviour
{
    [SerializeField] private int attackDamage;
    [SerializeField] private Element attackingElement;
    
    private UnitController unitController;
    private GridController gridController;
    
    public Vector2Int CurrentCell => (Vector2Int) gridController.Grid.WorldToCell(transform.position);

    private void Start()
    {
        unitController = ServiceLocator.Current.Get<UnitManager>().Controller;
        gridController = ServiceLocator.Current.Get<GridManager>().Controller;
    }

    public void AttackPlayer(PlayerUnit playerUnit)
    {
        // Attack animation
        unitController.CreateAttackEffect(playerUnit.CurrentCell, attackingElement);

        Health unitHealth = playerUnit.GetComponent<Health>();
        unitHealth.TakeDamage(attackDamage);
    }
}