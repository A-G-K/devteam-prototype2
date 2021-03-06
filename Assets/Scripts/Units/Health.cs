using System;
using UnityEngine;
using UnityEngine.Events;


public class Health : MonoBehaviour
{

    private int maxHealth;
    public int MaxHealth {get; set;}
    public UnityEvent onDeath;

    public int CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth == 0;

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);

        if (CurrentHealth == 0)
        {
            onDeath?.Invoke();
        }
    }

    public void TakeHeal(int amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);
    }
}
