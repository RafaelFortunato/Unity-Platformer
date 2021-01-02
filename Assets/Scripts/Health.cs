using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int MaxHealth;

    private int currentHealth;
    public int CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            OnHealthChanged?.Invoke(value);
        }
    }

    public bool IsAlive => currentHealth > 0;

    public event Action<int> OnHealthChanged;

    public float invincibilityTime;
    [HideInInspector] public float invincibilityCooldown;

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        if (invincibilityCooldown > 0)
        {
            invincibilityCooldown -= Time.deltaTime;
        }
    }
}
