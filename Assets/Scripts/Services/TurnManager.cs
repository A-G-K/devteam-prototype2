using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : IGameService
{
    public TurnController TurnController {get;set;}
    public TurnsUI TurnsUI {get;set;}

    public Turn currentTurn => TurnController.CurrentTurn; // access what the current turn is

    public void ChangeToPlayerTurn()
    {
        TurnsUI.ChangeToPlayerTurn();
    }

    public void ChangeToEnemyTurn()
    {
        TurnsUI.ChangeToEnemyTurn();
    }
}
