using System;
using UnityEngine;
using RoboRyanTron.Unite2017.Events;
using Services;

public enum elementType 
{
    Air,
    Water,
    Fire,
    Earth
}

public class Unit : MonoBehaviour
{   

    public PlayerData playerData;

    [SerializeField] private GameEvent newPlayer;

    public Health heathData;
    private int movementPoints;

    public int CurrentMovementPoints { get;  set; }
    public int ActionCount { get; set; }
    public bool CanMove => ActionCount > 0;
    public Vector2Int CurrentCell => (Vector2Int) unitController.Grid.WorldToCell(transform.position);

    private SpriteRenderer spriteRenderer;

    private Element elementType;

    private UnitController unitController;

    private void Awake()
    {   
        heathData = this.GetComponent<Health>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        newPlayer.Raise();
    

        CurrentMovementPoints = playerData.unit_MovementPoints;
        spriteRenderer.color = playerData.unit_Colour;
        elementType = playerData.elementType;
        heathData.MaxHealth = playerData.unit_MaxHealth;
        Debug.Log($"{playerData.unit_name} spawned!");
        
        ResetTokens();
    }

    private void Start()
    {
        unitController = ServiceLocator.Current.Get<UnitManager>().Controller;
        ActionCount = unitController.ActionCountPerTurn;
    }

    public void NextTurn()
    {
        CurrentMovementPoints = playerData.unit_MovementPoints;
        ActionCount = unitController.ActionCountPerTurn;
        ResetTokens();
    }

    public void ResetTokens()
    {
        playerData.currentTokens.Clear();
        
        for (int i = 0; i < playerData.unit_StartElementalToken; i++)
        {
            playerData.currentTokens.Add(playerData.elementType);
        }
    }
}