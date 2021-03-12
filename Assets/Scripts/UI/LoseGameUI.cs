using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseGameUI : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]private GameObject UI;
    void Start()
    {
        UI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DidLose() 
    {
         UI.SetActive(true);

    }
}
