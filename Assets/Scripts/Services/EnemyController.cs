using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    [SerializeField] private float delayBetweenEnemies = 1.5f;
    [SerializeField] private List<IEnemyBehaviour> enemies = new List<IEnemyBehaviour>();

    private bool isRunningEnemyTurn;
    
    public IEnumerable<IEnemyBehaviour> Enemies => enemies.AsReadOnly();

    private void Awake()
    {
        ServiceLocator.Current.Get<EnemyManager>().Controller = this;
    }

    private void Start()
    {
        SetAllEnemies();
    }

    public void SetAllEnemies() 
    {
               enemies = GameObject.FindGameObjectsWithTag("Enemy")
            .Select(e => e.GetComponent<IEnemyBehaviour>())
            .Where(e => e != null)
            .ToList();
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
        yield return new WaitForSeconds(3f);
        
        foreach (IEnemyBehaviour enemy in Enemies)
        {
            isRunningEnemyTurn = true;
            enemy.ActOnTurn();
            yield return new WaitWhile(() => isRunningEnemyTurn);
            yield return new WaitForSeconds(delayBetweenEnemies);
        }
        
        ServiceLocator.Current.Get<TurnManager>().TurnController.EndTurnButton();
        
        foreach (IEnemyBehaviour enemy in Enemies)
        {
            enemy.NextTurn();
        }
        
        yield return new WaitForSeconds(3f);
    }
}