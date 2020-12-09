using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private SkeletonAI skeletonAI;

    private const float attackDelay = 1f;
    private float attackCooldown = 0f;

    public AttackState(SkeletonAI skeletonAI)
    {
        this.skeletonAI = skeletonAI;
    }

    public void Update()
    {
        attackCooldown -= Time.deltaTime;

        if (skeletonAI.HasDelay())
        {
            return;
        }

        if (attackCooldown <= 0)
        {
            attackCooldown = attackDelay;
            skeletonAI.controlDelay = attackDelay;
            skeletonAI.animator.SetTrigger("Attack");
        }
    }

    public void OnEnter()
    {
        skeletonAI.OnAttackStrike += ApplyDamageToArea;
    }

    public void OnExit()
    {
        skeletonAI.OnAttackStrike -= ApplyDamageToArea;
    }

    private void ApplyDamageToArea()
    {
        Collider[] colliders = Physics.OverlapSphere(skeletonAI.swordAttackPosition.position, skeletonAI.attackRange);
        Debug.Log("Colliders count " + colliders.Length);
        foreach (var collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                DamageController.ApplyDamage(collider.gameObject, skeletonAI.attackDamage);
            }
        }
    }
}
