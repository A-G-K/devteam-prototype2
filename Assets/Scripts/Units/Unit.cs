using System;
using UnityEngine;


public class Unit : MonoBehaviour
{
    public int actionPoints;

    public int CurrentActionPoints { get; set; }

    private void Awake()
    {
        CurrentActionPoints = actionPoints;
    }

    public void NextTurn()
    {
        CurrentActionPoints = actionPoints;
    }
}