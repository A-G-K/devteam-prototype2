using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : IGameService
{
    public TurnManager turnManager {get;set;}

    public Turn currentTurn => turnManager.CurrentTurn; // access what the current turn is

}
