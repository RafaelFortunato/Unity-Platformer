using System;
using System.Collections;
using UnityEngine;

public class DamageController
{
    public static void ApplyDamage(GameObject gameObject, int damage)
    {
        var healthComponent = gameObject.GetComponent<Health>();
        if (healthComponent == null || healthComponent.invencibilityCooldown > 0)
            return;

        healthComponent.CurrentHealth = Math.Max(0, healthComponent.CurrentHealth - damage);

        if (healthComponent.invencibilityTime > 0)
        {
            healthComponent.invencibilityCooldown = healthComponent.invencibilityTime;
            var worldCharacter = gameObject.GetComponent<WorldCharacter>();
            if (worldCharacter != null)
            {
                worldCharacter.DamageTakenAnimation(healthComponent.invencibilityTime);
            }
        }


        if (healthComponent.CurrentHealth <= 0)
        {
            if (gameObject.CompareTag("Enemy"))
            {
                GameObject.Destroy(gameObject);
            }
            else if (gameObject.CompareTag("Player"))
            {
                // TODO: GameOver
            }
        }
    }
}
