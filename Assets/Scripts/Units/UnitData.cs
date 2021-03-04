using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum elementType 
{
    Air,
    Water,
    Fire,


}

[CreateAssetMenu(fileName = "PlayerUnit", menuName="PlayerUnits/CreateNewUnit")]
public class UnitData : ScriptableObject
{   

    [Header("Unit Information")]
    public string unit_name;

    public Sprite unit_Sprite;
    public elementType element_Type;

    

    [Header("Unit Stats")]


    public int elemntalToken_Start;

    public int elementalToken_Current;

    public int movement_points;






}

