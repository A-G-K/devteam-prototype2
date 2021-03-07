using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class AbilityStats : ScriptableObject
{
    public Element element;
    public Sprite icon;
    
//#if UNITY_EDITOR
    [Multiline]
    public string Description = "";
//#endif

    public int damage; // Negative damage = healing
    public int range = 1;
    public int radius = 1;
    public List<Element> elementalCost;

    // // probs put unit as input parameters
    // public virtual void ProcessAbility()
    // {
    //     
    // }
    
}
