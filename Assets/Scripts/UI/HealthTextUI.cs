using System.Collections;
using System.Collections.Generic;
using Services;
using TMPro;
using UnityEngine;

public class HealthTextUI : MonoBehaviour
{
    private UnitManager _unitManager;
    private TextMeshProUGUI _text;
    
    // Start is called before the first frame update
    void Awake()
    {
        _unitManager = ServiceLocator.Current.Get<UnitManager>();
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void OnPlayerUnitSelect()
    {
        _text.text = "Health: " + _unitManager.Controller.SelectedPlayerUnit.heathData.CurrentHealth;
        _text.enabled = true;
    }

    public void OnPlayerUnitDeselect()
    {
        _text.enabled = false;
    }
}
