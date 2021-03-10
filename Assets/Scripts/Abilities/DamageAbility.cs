using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Services;
using RoboRyanTron.Unite2017.Events;

public class DamageAbility : MonoBehaviour
{
    [SerializeField] private AbilityStats _abilityStats;

    [SerializeField] private TextMeshProUGUI abilityNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image abilityImage;
    [SerializeField] private GameObject tokensHolder;
    [SerializeField] [HideInInspector] private CanvasGroup _canvasGroup;
 



    

    private bool isUsable = true;
    // Start is called before the first frame update
    void Start()
    {


    }

    public void OnUnitSelect()
    {


       


        if (isUsable)
        {
            ActivateAbility();
        }
    }


    public void ActivateAbility()
    {
        Debug.Log($"Card Clicked");
        // Show range
        // Process Ability on clicking valid target
        // disable card
    }

    void Update() 
    {
       
    }
    
    public void ProcessAbility(GameObject user, GameObject target)
    {
        List<GameObject> targets = new List<GameObject> {target};

        if (isUsable)
        {
            
        }
        // // calculate range between user and target
        // if (target.pos - user.pos < _abilityStats.range)
        // {
        //     if (_abilityStats.radius > 1)
        //     {
        //         // Get all units within the target radius
        //         // Add them into the list
        //     }
        //
        //     foreach (var enemy in targets)
        //     {
        //         // Calculate how much damage to do
                    // int damage = (int) (_abilityStats.damage * _abilityStats.element.AttackWithThisElement(enemy.element));
                    //  
                    // it deals damage
                    // // enemy.health -= damage;
        //     }
        // }
        // else
        // {
        //     Debug.Log("Enemy not in range");
        // }
    }

    private void OnEnable()
    {
        abilityNameText.text = _abilityStats.name;
        descriptionText.text = _abilityStats.Description;
        abilityImage.sprite = _abilityStats.icon;
        
        Image[] tokenSprites = tokensHolder.GetComponentsInChildren<Image>(true);
        for (int i = 0; i < _abilityStats.elementalCost.Count; i++)
        {
            tokenSprites[i].gameObject.SetActive(true);
            tokenSprites[i].sprite = _abilityStats.elementalCost[i].icon;
        }
        
        // Check if selected unit has enough tokens, if so set isUsable to true else false.
        _canvasGroup.alpha = isUsable ? 1f : 0.7f;
    }

    private void OnDisable()
    {
        Image[] tokenSprites = tokensHolder.GetComponentsInChildren<Image>();
        for (int i = 0; i < _abilityStats.elementalCost.Count; i++)
        {
            tokenSprites[i].gameObject.SetActive(false);
        }
    }

    private void OnValidate()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public AbilityStats GetAbilityStats() 
    {
        return _abilityStats;
    }
}