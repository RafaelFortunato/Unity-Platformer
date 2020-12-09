using System;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

public class SkeletonAI : MonoBehaviour
{
    public float aggroRange;
    public float attackRange;
    public int attackDamage;
    public float walkSpeed;
    public Transform swordAttackPosition;

    [HideInInspector] public Animator animator;
    [HideInInspector] public Rigidbody rigidbody;
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public event Action OnAttackStrike;
    [HideInInspector] public event Action OnAttackFinished;
    [HideInInspector] public float controlDelay;

    public Transform groundAheadCheck;
    public Transform groundBelowCheck;
    public LayerMask groundLayer;  // A mask determining what is ground to the character


    private StateMachine stateMachine = new StateMachine();

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        var patrolState = new PatrolState(this);
        var chasePlayerState = new ChasePlayerState(this);
        var attackState = new AttackState(this);

        stateMachine.AddTransition(patrolState, chasePlayerState, PlayerInAggroRange);
        stateMachine.AddAnyTransition(attackState, PlayerInAttackRange);
        stateMachine.AddAnyTransition(patrolState, () => !PlayerInAggroRange());
        stateMachine.AddTransition(attackState, chasePlayerState, () => !PlayerInAttackRange());

        stateMachine.SetState(patrolState);
    }

    bool PlayerInAggroRange() => Vector3.Distance(transform.position, playerTransform.position) <= aggroRange;
    bool PlayerInAttackRange() => Vector3.Distance(swordAttackPosition.position, playerTransform.position) <= attackRange;

    void Update()
    {
        controlDelay -= Time.deltaTime;
        stateMachine.Update();
    }

    public void AttackStrike()
    {
        OnAttackStrike?.Invoke();
    }

    public void AttackFinished()
    {
        //stateMachine.SetState(new ChasePlayerState(this));
    }

    public bool HasDelay() => controlDelay > 0;

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
