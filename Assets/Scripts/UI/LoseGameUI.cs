using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseGameUI : MonoBehaviour
{

    [SerializeField] private GameObject loseGameUI;

    void Start() 
    {
        loseGameUI.SetActive(false);
    }

    void Update() 
    {

    }

    public void DidLose() 
    {
        loseGameUI.SetActive(true);
    }

    public void PlayAgin() 
    {
        SceneManager.LoadScene("Baby Don't Hurt Me");
    }   
    
}
