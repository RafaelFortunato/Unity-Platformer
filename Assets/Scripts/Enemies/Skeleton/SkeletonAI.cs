using System;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class SkeletonAI : BaseAI
{
    [Header("Sword Attack")]
    public float aggroRange;
    public float attackRange;
    public float attackDelay;
    public int attackDamage;
    public Transform swordAttackPosition;

    [Header("Sounds")]
    public AudioSource attackSound;

    [HideInInspector] public Transform playerTransform;
    public UnityEvent onAttackStrike;

    new void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        var patrolState = new BasePatrolState(this);

        ChasePlayerStateModel chasePlayerStateModel = new ChasePlayerStateModel
        {
            playerTransform = playerTransform
        };
        var chasePlayerState = new ChasePlayerState(this, chasePlayerStateModel);

        AttackStateModel attackStateModel = new AttackStateModel
        {
            transform = swordAttackPosition,
            range = attackRange,
            delay = attackDelay,
            damage = attackDamage,
            onAttackStrike = onAttackStrike,
            attackSound = attackSound
        };

        var attackState = new BaseAttackState(this, attackStateModel);

        stateMachine.AddTransition(patrolState, chasePlayerState, PlayerInAggroRange);
        stateMachine.AddAnyTransition(attackState, PlayerInAttackRange);
        stateMachine.AddAnyTransition(patrolState, () => !PlayerInAggroRange());
        stateMachine.AddTransition(attackState, chasePlayerState, () => !PlayerInAttackRange());

        stateMachine.SetState(patrolState);
    }

    bool PlayerInAggroRange() => Vector3.Distance(transform.position, playerTransform.position) <= aggroRange;
    bool PlayerInAttackRange() => Vector3.Distance(swordAttackPosition.position, playerTransform.position) <= attackRange;

    public void AttackStrike()
    {
        onAttackStrike.Invoke();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.forward, aggroRange);
        Handles.color = Color.white;
        Handles.DrawWireDisc(swordAttackPosition.position, Vector3.forward, attackRange);
    }
#endif
}
