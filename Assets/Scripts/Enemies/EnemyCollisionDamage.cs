using System;
using UnityEngine;

public class EnemyCollisionDamage : MonoBehaviour
{
    public int damage;

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DamageController.ApplyDamage(other.gameObject, damage);
        }
    }
}
