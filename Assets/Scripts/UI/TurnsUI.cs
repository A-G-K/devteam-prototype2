using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnsUI : MonoBehaviour
{
    [SerializeField] private Text txtTurnIndicator;

    [SerializeField] private Text txtButtonText;

    [SerializeField] private Button btnEndTurn;

    private void Awake()
    {
        ServiceLocator.Current.Get<TurnManager>().TurnsUI = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        txtTurnIndicator.color = new Color32(0,255,0,255);
        txtTurnIndicator.text = "PLAYER'S TURN";
        txtButtonText.text = "END TURN";
    }

    public void ChangeToPlayerTurn()
    {
        txtTurnIndicator.color = new Color32(0,255,0,255);
        txtTurnIndicator.text = "PLAYER'S TURN";
        txtButtonText.fontSize = txtButtonText.fontSize * 2;
        txtButtonText.text = "END TURN";
        btnEndTurn.enabled = true;
    }

    public void ChangeToEnemyTurn()
    {
        txtTurnIndicator.color = new Color32(255,0,0,255);
        txtTurnIndicator.text = "ENEMY'S TURN";
        txtButtonText.fontSize = txtButtonText.fontSize / 2;
        txtButtonText.text = "WAIT FOR ENEMY'S TURN";
        btnEndTurn.enabled = false;
    }
}
