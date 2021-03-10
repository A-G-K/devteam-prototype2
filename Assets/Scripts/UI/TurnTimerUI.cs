using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnTimerUI : MonoBehaviour
{
    [SerializeField] private string _turnText;
    //private TurnManager _turnManager;
    private TextMeshProUGUI _text;

    private int _turns = 1;
    // Start is called before the first frame update
    void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        //_turnManager = ServiceLocator.Current.Get<TurnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEndTurn()
    {
        _turns++;
        _text.text = _turnText + " " + _turns;
    }
}
