using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerUnit", menuName="Units/Enemy Unit")]
public class EnemyData : UnitData
{   
    [Header("Enemy Information")]
    public int EnemyDamage;
}
