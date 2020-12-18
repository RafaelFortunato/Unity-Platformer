using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartItem : MonoBehaviour
{
    public int recoveryAmount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var healthComponent = other.gameObject.GetComponent<Health>();
            if (healthComponent == null)
                return;

            healthComponent.CurrentHealth = Math.Min(healthComponent.CurrentHealth + recoveryAmount, healthComponent.MaxHealth);

            Destroy(gameObject);
        }
    }
}
