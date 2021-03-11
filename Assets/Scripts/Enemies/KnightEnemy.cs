using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


public class KnightEnemy : MonoBehaviour, IEnemyBehaviour
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
        DoActOnTurn();
    }

    private async Task DoActOnTurn()
    {
        await enemyMovement.MoveTo(enemyMovement.TowardsNearestPlayerUnit());
        IEnumerable<PlayerUnit> nearbyPlayerUnits = enemyMovement.GetNearbyPlayerUnits();
        PlayerUnit anyPlayerPlayerUnit = nearbyPlayerUnits.FirstOrDefault();

        // This means the enemy is next to the player unit
        if (anyPlayerPlayerUnit != null)
        {
            enemyAttacker.AttackPlayer(anyPlayerPlayerUnit);
        }
        
        enemyController.PassNextEnemyTurn();
    }

    public void NextTurn()
    {
        enemyMovement.ResetMovement();
    }
}