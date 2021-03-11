using UnityEngine;


public interface IUnit
{
    // We allow this from Unity MonoBehaviour.transform to be used
    Transform transform { get; }
    
    Health Health { get; }
    
    Element Element { get; }
}