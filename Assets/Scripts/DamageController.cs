using System;
using UnityEngine;

public class DamageController
{
    public static void ApplyDamage(GameObject gameObject, int damage)
    {
        var healthComponent = gameObject.GetComponentInParent<Health>();
        if (healthComponent == null || healthComponent.invincibilityCooldown > 0 || healthComponent.CurrentHealth <= 0)
        {
            return;
        }

        healthComponent.CurrentHealth = Math.Max(healthComponent.CurrentHealth - damage, 0);

        ApplyInvincibilityTime(gameObject, healthComponent);

        HandleEntityDeath(gameObject, healthComponent);
    }

    private static void ApplyInvincibilityTime(GameObject gameObject, Health healthComponent)
    {
        if (healthComponent.invincibilityTime <= 0)
        {
            return;
        }

        healthComponent.invincibilityCooldown = healthComponent.invincibilityTime;
        var worldCharacter = gameObject.GetComponent<WorldCharacter>();
        if (worldCharacter != null)
        {
            worldCharacter.DamageTakenAnimation(healthComponent.invincibilityTime);
        }
    }

    private static void HandleEntityDeath(GameObject gameObject, Health healthComponent)
    {
        if (healthComponent.CurrentHealth > 0)
        {
            return;
        }

        if (gameObject.CompareTag(Tags.ENEMY))
        {
            CollisionEventHandler enemyObject = gameObject.GetComponent<CollisionEventHandler>();
            enemyObject.baseAI.DeathAnimation();
        }
        else if (gameObject.CompareTag(Tags.PLAYER))
        {
            PlayerController playerController = gameObject.GetComponent<PlayerController>();
            playerController.Death();
        }
    }
}