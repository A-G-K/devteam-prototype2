using System;
using UnityEngine;
using UnityEngine.Events;


public class Health : MonoBehaviour
{
    private int maxHealth;

    public int MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

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
        amount = Mathf.Abs(amount);
        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);
    }
}
