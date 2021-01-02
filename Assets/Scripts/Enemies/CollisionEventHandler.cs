using System;
using UnityEngine;

public class CollisionEventHandler : MonoBehaviour
{
    public BaseAI baseAI;
    public Health healthComponent;

    public int damage;

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && healthComponent.IsAlive)
        {
            DamageController.ApplyDamage(other.gameObject, damage);
        }
    }
}
