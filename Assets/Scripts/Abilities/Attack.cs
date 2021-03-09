using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private Element _attackingElement;
    [SerializeField] private Element _defendingElement;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(_defendingElement + " " + _defendingElement.AttackWithThisElement(_attackingElement));
        Debug.Log(_attackingElement + " " + _attackingElement.AttackWithThisElement(_defendingElement));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
