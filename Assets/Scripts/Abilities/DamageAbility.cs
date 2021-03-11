using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Services;
using RoboRyanTron.Unite2017.Events;
using UnityEngine.EventSystems;

public class DamageAbility : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private AbilityStats _abilityStats;

    [SerializeField] private TextMeshProUGUI abilityNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image abilityImage;
    [SerializeField] private GameObject tokensHolder;
    [SerializeField] [HideInInspector] private CanvasGroup _canvasGroup;

    [SerializeField]
    private GameEvent selectAbility;
    private AbilityUIManager _abilityUIManager;


    private bool isUsable;

    // Start is called before the first frame update
    void Start()
    {
        _abilityUIManager = ServiceLocator.Current.Get<AbilityUIManager>();
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
        _abilityUIManager.AbilityController.curSelectedAbility = _abilityStats;
        selectAbility.Raise();
        Debug.Log($"Card Clicked");
        
        // Show range
        // Process Ability on clicking valid target
        // disable card
    }

    void Update()
    {
    }

    public void ProcessAbility(GameObject target)
    {
        List<GameObject> targets = new List<GameObject> {target};
        UnitController user = ServiceLocator.Current.Get<UnitManager>().Controller;

        if (isUsable)
        {
            isUsable = false;
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
        PlayerData playerData = ServiceLocator.Current.Get<UnitManager>().Controller.SelectedUnit.playerData;
        abilityNameText.text = _abilityStats.name;
        descriptionText.text = _abilityStats.Description;
        abilityImage.sprite = _abilityStats.icon;

        Image[] tokenSprites = tokensHolder.GetComponentsInChildren<Image>(true);
        for (int i = 0; i < _abilityStats.elementalCost.Count; i++)
        {
            tokenSprites[i].gameObject.SetActive(true);
            tokenSprites[i].sprite = _abilityStats.elementalCost[i].icon;
        }

        if (_abilityStats.elementalCost.All(playerData.currentTokens.Contains))
        {
            isUsable = true;
            Debug.Log("Tokens exists");
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

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (eventData.pointerId)
        {
            // Left click
            case -1:
                if (isUsable)
                {
                    ActivateAbility();
                }
                break;
            case -2:
                // something hehe
                break;
        }
    }
}