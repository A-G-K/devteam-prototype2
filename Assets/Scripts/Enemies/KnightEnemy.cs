using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class KnightEnemy : MonoBehaviour, IEnemy
{
    private EnemyController enemyController;
    private EnemyMovement enemyMovement;
    private EnemySimpleAttacker enemyAttacker;

    private void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        enemyAttacker = GetComponent<EnemySimpleAttacker>();
        enemyController = ServiceLocator.Current.Get<EnemyManager>().Controller;
    }

    public void ActOnTurn()
    {
        enemyMovement.MoveTo(enemyMovement.TowardsNearestPlayerUnit());
        IEnumerable<Unit> nearbyPlayerUnits = enemyMovement.GetNearbyPlayerUnits();
        Unit anyPlayerUnit = nearbyPlayerUnits.FirstOrDefault();

        // This means the enemy is next to the player unit
        if (anyPlayerUnit != null)
        {
            enemyAttacker.AttackEnemy(anyPlayerUnit);
        }
        
        enemyController.PassNextEnemyTurn();
    }

    public void NextTurn()
    {
        enemyMovement.ResetMovement();
    }
}