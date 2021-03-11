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

    [SerializeField] private Text txtTurnIndicator;

    [SerializeField] private Text txtButtonText;

    [SerializeField] private Button btnEndTurn;


        public Turn CurrentTurn {get; set;}


    private UnitController unitController;

    public void EndTurnButton() => EndTurn.Raise();

    public int roundCounter = 1;

    
    void Awake() 
    {
            ServiceLocator.Current.Get<TurnManager>().TurnController = this;
            CurrentTurn = Turn.Player;
            
    }

    void Start() 
    {
        unitController = ServiceLocator.Current.Get<UnitManager>().Controller;
        txtTurnIndicator.color = new Color32(0,255,0,255);
        txtTurnIndicator.text = "PLAYER'S TURN";
        txtButtonText.text = "END TURN";
    }




    public void ChangeTurn() 
    {

        // UI CHANGES
        if (CurrentTurn == Turn.Player) 
        {
            CurrentTurn = Turn.Enemy;
            txtTurnIndicator.color = new Color32(255,0,0,255);
            txtTurnIndicator.text = "ENEMY'S TURN";
            txtButtonText.fontSize = txtButtonText.fontSize / 2;
            txtButtonText.text = "WAIT FOR ENEMY'S TURN";
            btnEndTurn.enabled = false;
            unitController.enabled = false;
            StartCoroutine(timer()); // for debugging

        } else 
        {
            CurrentTurn = Turn.Player;
            txtTurnIndicator.color = new Color32(0,255,0,255);
            txtTurnIndicator.text = "PLAYER'S TURN";
            txtButtonText.fontSize = txtButtonText.fontSize * 2;
            txtButtonText.text = "END TURN";
            btnEndTurn.enabled = true;
            unitController.enabled = true;

            roundCounter++;
            NewRound.Raise();
            foreach (Unit unit in unitController.allPlayerUnits) 
            
            {
                unit.NextTurn();
            }



        }

        Debug.Log($"======>TURN HAS CHANGED TO {CurrentTurn}");

    }

    IEnumerator timer() 
    {
        yield return new WaitForSeconds(2f);
        ChangeTurn();
    }




  




 
    



}


