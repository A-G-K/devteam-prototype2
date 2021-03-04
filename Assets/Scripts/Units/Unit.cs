using System;
using UnityEngine;


public class Unit : MonoBehaviour
{   

    public UnitData unitData;
    private int movementPoints;

    public int CurrentMovementPoints { get; set; }

    public Transform trans;

    private void Awake()
    {
        CurrentMovementPoints = unitData.movement_points;

    }

    public void NextTurn()
    {
        CurrentMovementPoints = unitData.movement_points;
    }
}