using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Element : ScriptableObject
{
    public Sprite icon;
    
    [Tooltip("The elements that this element is weak to.")]
    public List<Element> weakAgainstElements = new List<Element>();

    /*
     * Try to find a way where 
     */
    [Tooltip("The elements that this element is strong to.")]
    public List<Element> strongAgainstElements = new List<Element>();

    public float AttackWithThisElement(Element defendingElement)
    {
        if (weakAgainstElements.Contains(defendingElement))
        {
            return 0.5f;
        }
        if (strongAgainstElements.Contains(defendingElement))
        {
            return 2f;
        }

        return 1f;
    }
}
