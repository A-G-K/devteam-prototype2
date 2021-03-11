using System.Collections;
using System.Collections.Generic;
using Services;
using TMPro;
using UnityEngine;

public class MovementPointTextUI : MonoBehaviour
{
    private UnitManager _unitManager;
    private TextMeshProUGUI _text;
    
    // Start is called before the first frame update
    void Start()
    {
        _unitManager = ServiceLocator.Current.Get<UnitManager>();
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void OnPlayerUnitSelect()
    {
        _text.text = "MP: " + _unitManager.Controller.SelectedPlayerUnit.CurrentMovementPoints;
        _text.enabled = true;
    }

    public void OnPlayerUnitDeselect()
    {
        _text.enabled = false;
    }
}
