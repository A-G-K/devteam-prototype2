using System;
using Services;
using UnityEngine;


public class EnemySimpleAttacker : MonoBehaviour
{
    [SerializeField] private int attackDamage;
    
    private UnitController unitController;
    
    public Vector2Int CurrentCell => (Vector2Int) unitController.Grid.WorldToCell(transform.position);

    private void Start()
    {
        unitController = ServiceLocator.Current.Get<UnitManager>().Controller;
    }

    public void AttackEnemy(Unit playerUnit)
    {
        // TODO sprite attack animation, sounds or whatever

        Health unitHealth = playerUnit.GetComponent<Health>();
        unitHealth.TakeDamage(attackDamage);
    }
}