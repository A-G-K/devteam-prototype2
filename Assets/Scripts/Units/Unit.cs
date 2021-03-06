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

    private elementType elementType;

    private void Awake()
    {   
        heathData = this.GetComponent<Health>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();


        CurrentMovementPoints = playerData.unit_MovementPoints;
        spriteRenderer.color = playerData.unit_Colour;
        elementType = playerData.elemental_Type;
        heathData.MaxHealth = playerData.unit_MaxHealth;
        Debug.Log($"{playerData.unit_name} spawned!");

    }

    public void NextTurn()
    {
        CurrentMovementPoints = playerData.unit_MovementPoints;
    }
}