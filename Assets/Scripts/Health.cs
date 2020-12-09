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

    public event Action<int> OnHealthChanged;

    public float invencibilityTime;
    [HideInInspector] public float invencibilityCooldown;

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        invencibilityCooldown -= Time.deltaTime;
    }
}
