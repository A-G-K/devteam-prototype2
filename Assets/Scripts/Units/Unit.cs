using System;
using UnityEngine;

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

    private Health heathData;
    private int movementPoints;

    public int CurrentMovementPoints { get;  set; }

    public Transform trans;

    private SpriteRenderer spriteRenderer;

    private Element elementType;

    private void Awake()
    {   
        heathData = this.GetComponent<Health>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();


        CurrentMovementPoints = playerData.unit_MovementPoints;
        spriteRenderer.color = playerData.unit_Colour;
        elementType = playerData.elementType;
        heathData.MaxHealth = playerData.unit_MaxHealth;
        Debug.Log($"{playerData.unit_name} spawned!");
        
        ResetTokens();
    }

    public void NextTurn()
    {
        CurrentMovementPoints = playerData.unit_MovementPoints;
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