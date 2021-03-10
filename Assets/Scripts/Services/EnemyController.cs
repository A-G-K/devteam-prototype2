using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    [SerializeField] private float delayBetweenEnemies = 1.5f;
    [SerializeField] private List<IEnemy> enemies = new List<IEnemy>();

    private bool isRunningEnemyTurn;
    
    public IEnumerable<IEnemy> Enemies => enemies.AsReadOnly();

    private void Awake()
    {
        ServiceLocator.Current.Get<EnemyManager>().Controller = this;
    }

    /// <summary>
    /// Enemy implementations need to call this function once the enemy is done with movement to be able to continue the turns.
    /// </summary>
    public void PassNextEnemyTurn()
    {
        isRunningEnemyTurn = false;
    }

    public void OnEnemyTurn()
    {
        StartCoroutine(DoEnemyTurns());
    }

    private IEnumerator DoEnemyTurns()
    {
        foreach (IEnemy enemy in Enemies)
        {
            isRunningEnemyTurn = true;
            enemy.ActOnTurn();
            yield return new WaitWhile(() => isRunningEnemyTurn);
            yield return new WaitForSeconds(delayBetweenEnemies);
        }
        
        ServiceLocator.Current.Get<TurnManager>().TurnController.EndTurnButton();
        
        foreach (IEnemy enemy in Enemies)
        {
            enemy.NextTurn();
        }
    }
}