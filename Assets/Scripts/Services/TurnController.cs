using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using RoboRyanTron.Unite2017.Events;

public enum Turn
{
    Player,
    Enemy
}


public class TurnController : MonoBehaviour
{
    [SerializeField] private GameEvent EndTurn;
    [SerializeField] private GameEvent NewRound;

    [SerializeField] private GameEvent EnemyTurn;

    [SerializeField] private GameEvent PlayerTurn;

    public Turn CurrentTurn {get; set;}

    public int roundCounter;

    private UnitController unitController;
    private bool isChangingTurn;

    private TurnManager _turnManager;
    public void EndTurnButton()
    {
        if (!isChangingTurn)
        {
            EndTurn.Raise();
        }
        else
        {
            StartCoroutine(WaitAndEndTurn());
        }
    }

    private IEnumerator WaitAndEndTurn()
    {
        yield return new WaitWhile(() => isChangingTurn);
        yield return null;
        EndTurn.Raise();
    }

    private void Awake() 
    {
        ServiceLocator.Current.Get<TurnManager>().TurnController = this;
        CurrentTurn = Turn.Player;
    }

    private void Start() 
    {
        unitController = ServiceLocator.Current.Get<UnitManager>().Controller;
        _turnManager = ServiceLocator.Current.Get<TurnManager>();
    }

    public void ChangeTurn()
    {
        isChangingTurn = true;
        

        // UI CHANGES
        if (CurrentTurn == Turn.Player) 
        {
            CurrentTurn = Turn.Enemy;
            
            unitController.enabled = false;
             //StartCoroutine(DelayAndChangeTurn()); // for debugging
            _turnManager.ChangeToEnemyTurn();
            EnemyTurn.Raise();
        } 
        else 
        {
            CurrentTurn = Turn.Player;
            
            unitController.enabled = true;

            roundCounter++;
            NewRound.Raise();
            foreach (PlayerUnit unit in unitController.allPlayerUnits) 
            
            {
                unit.NextTurn();
            }

            _turnManager.ChangeToPlayerTurn();
            PlayerTurn.Raise();
        }

        Debug.Log($"======>TURN HAS CHANGED TO {CurrentTurn}");
        isChangingTurn = false;
    }

    private IEnumerator DelayAndChangeTurn()
    {
        yield return DelayAndChangeTurn(2f);
    }
    
    private IEnumerator DelayAndChangeTurn(float seconds) 
    {
        yield return new WaitForSeconds(seconds);
        ChangeTurn();
    }
}


