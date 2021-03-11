using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnTimerUI : MonoBehaviour
{
    [SerializeField] private string _turnText;

    private TurnController _turnController;
    private TextMeshProUGUI _text;

    // Start is called before the first frame update
    void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        _turnController = ServiceLocator.Current.Get<TurnManager>().TurnController;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnNewRound()
    {
        int turns = _turnController.roundCounter;
        _text.text = _turnText + " " + turns;
    }
}
