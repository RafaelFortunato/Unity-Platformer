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

    public GameObject destructionParticles;

    public Animator animator;
    [HideInInspector] public Rigidbody rigidbody;
    [HideInInspector] public float controlDelay;

    protected StateMachine stateMachine = new StateMachine();

    protected void Start()
    {
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

    public virtual void DeathAnimation()
    {
        controlDelay = 3;
        animator.SetTrigger("Dead");
        Invoke("DestroyAnimation", 1);
    }

    public void DestroyAnimation()
    {
        Destroy(gameObject);
        Instantiate(destructionParticles, transform.position, Quaternion.identity);
        destructionParticles.transform.position = transform.position;
    }
}