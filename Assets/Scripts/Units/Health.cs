using System;
using UnityEngine;
using UnityEngine.Events;


public class Health : MonoBehaviour
{
    [SerializeField] private int health = 1;
    [SerializeField] private int maxHealth = 1;
    public UnityEvent onDeath;

    public int CurrentHealth { get; private set; }
    public bool IsDead => health == 0;

    private void Awake()
    {
        CurrentHealth = health;
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
