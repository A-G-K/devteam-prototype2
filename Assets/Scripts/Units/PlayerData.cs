using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elemental_Types;


[CreateAssetMenu(fileName = "PlayerUnit", menuName="Units/Player Unit")]
public class PlayerData : UnitData
{
    
    [Header("Player Unit Information")]
    public elementType elemental_Type;
    
    public int unit_MovementPoints;

    public int unit_StartElementalToken;


}
