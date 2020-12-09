using UnityEngine;

public class BettleAI : MonoBehaviour
{
    public float walkSpeed;
    public Transform groundAheadCheck;
    public Transform groundBelowCheck;
    public LayerMask groundLayer;  // A mask determining what is ground to the character

    [HideInInspector] public Animator animator;
    [HideInInspector] public Rigidbody rigidbody;
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public float controlDelay;

    private StateMachine stateMachine = new StateMachine();

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        var patrolState = new BettlePatrolState(this);

        stateMachine.SetState(patrolState);
    }

    void Update()
    {
        controlDelay -= Time.deltaTime;
        stateMachine.Update();
    }

    public bool HasDelay() => controlDelay > 0;
}