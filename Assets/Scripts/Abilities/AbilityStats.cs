using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class AbilityStats : ScriptableObject
{
    public GameObject abilityPrefab;
    [Multiline]
    public string Description = "";

    public Element element;
    public Sprite icon;

    [Header("Stats")]
    public int damage; // Negative damage = healing
    public int range = 1;
    public int radius = 1;
    public List<Element> elementalCost;
}
