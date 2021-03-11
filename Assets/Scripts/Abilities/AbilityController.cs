using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Events;
using Services;

public class AbilityController : MonoBehaviour
{

    private UnitController unitController;

    [SerializeField] private GameEvent selectedAbility;

    private UnitMoveVisualizer unitMoveVisualizer;


    public AbilityStats curSelectedAbility;


    public bool isAbilitySelected = false;

    private UnitManager unitManager;


    private void Awake()
    {
        ServiceLocator.Current.Get<AbilityUIManager>().AbilityController = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        unitController = ServiceLocator.Current.Get<UnitManager>().Controller;
        unitMoveVisualizer = ServiceLocator.Current.Get<UnitManager>().Visualizer;
       
    }

    // Update is called once per frame
    void Update()
    {

        if (unitController.SelectedUnit != null) 
        {
            int i =0;
            foreach (var ability in unitController.SelectedUnit.playerData.abilitiesList) 
            {
                i++;
                if (Input.inputString == i.ToString() && ability != curSelectedAbility) 
                {

                    foreach(GameObject abilityCards in GameObject.FindGameObjectsWithTag("AbilityCard")) 
                    {
                        if (abilityCards.GetComponent<DamageAbility>().GetAbilityStats() == ability) 
                        {
                            Debug.Log("CHEEFF");
                            curSelectedAbility = abilityCards.GetComponent<DamageAbility>().GetAbilityStats();
                            selectedAbility.Raise();

                        
                        }
                    }
                } 
            }

        }
    }   



    public void showAbilityRange()
    {
        unitMoveVisualizer.HighlightAbilityRange(curSelectedAbility.range); 
    }

    public void abilitySelected() 
    {
        isAbilitySelected = true;
    }

    public void OnUnitDeselected () 
    {
        curSelectedAbility = null;
        isAbilitySelected = false;
    }

   
}