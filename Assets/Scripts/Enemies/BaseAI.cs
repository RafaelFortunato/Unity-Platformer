using System;
using UnityEngine;

public class BaseAI : MonoBehaviour
{
    public float walkSpeed;

    public Transform groundAheadCheck;
    public Transform groundBelowCheck;
    public LayerMask groundLayer;

    public Transform obstacleAheadCheck;
    public LayerMask obstacleLayer;

    [HideInInspector] public Animator animator;
    [HideInInspector] public Rigidbody rigidbody;
    [HideInInspector] public float controlDelay;

    protected StateMachine stateMachine = new StateMachine();

    protected void Start()
    {
        if (!TryGetComponent(out animator))
        {
            animator = GetComponentInChildren<Animator>();
        }

        rigidbody = GetComponent<Rigidbody>();
    }

    protected void Update()
    {
        if (HasDelay())
        {
            controlDelay -= Time.deltaTime;
            return;
        }

        stateMachine.Update();
    }

    public bool HasDelay() => controlDelay > 0;
}