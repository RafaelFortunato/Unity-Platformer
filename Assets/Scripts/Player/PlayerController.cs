using System;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Sword Attack")]
    public int swordAttackDamage;
    public float SwordAttackRange;
    public float SwordAttackDelay;
    public Transform SwordAttackPosition;

    [Header("Fire Ball")]
    public GameObject fireballPrefab;
    public float MagicAttackDelay;

    [Header("Jump")]
    public float JumpHeight;
    public float gravityScale;
    public float jumpPressTime;

    [Header("Other")]
    public float WalkSpeed;
    public Animator anim;
    public LayerMask EnemyLayers;
    public LayerMask GroundLayer;  // A mask determining what is ground to the character

    private Rigidbody rigidbody;
    private bool isJumping = false;
    private float jumpTimeCounter = 0;
    private float ControlDelay = 0f;

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private Transform GroundCheck;    // A position marking where to check if the player is grounded.

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        GroundCheck = transform.Find("GroundCheck");
    }

    // Update is called once per frame
    void Update()
    {
        ControlDelay -= Time.deltaTime;

        if (!CanMove())
        {
            anim.SetBool("Jumping", !IsGrounded());

            if (!IsGrounded())
            {
                InputMovement();
            }
            return;
        }

        InputMovement();
        InputJump();
        InputAttack();
        InputMagicAttack();
    }

    private void InputMagicAttack()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ControlDelay = MagicAttackDelay;
            anim.SetTrigger("Magic");
        }
    }

    public void FireballAction()
    {
        Instantiate(fireballPrefab, SwordAttackPosition.position, SwordAttackPosition.rotation);
    }

    private bool CanMove()
    {
        return ControlDelay <= 0;
    }

    private void InputMovement()
    {
        float x = Input.GetAxis("Horizontal");

        // Debug.Log(x);
        //
        // Vector3 parentPos = Vector3.zero;
        // if (transform.parent != null)
        // {
        //     parentPos = transform.parent.GetComponent<Transform>().position;
        // }

        if (transform.parent == null)
        {
            rigidbody.MovePosition(rigidbody.position + new Vector3(x * WalkSpeed * Time.deltaTime, 0, 0));
        }
        else
        {
            transform.position += new Vector3(x * WalkSpeed * Time.deltaTime, 0, 0);
        }


        if (x < 0)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else if (x > 0)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        anim.SetFloat("Speed", Math.Abs(x));
    }


    void InputJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            isJumping = true;
            jumpTimeCounter = jumpPressTime;
            rigidbody.velocity = Vector3.up * JumpHeight;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rigidbody.velocity = Vector2.up * JumpHeight;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }

        anim.SetBool("Jumping", !IsGrounded());
    }

    private void InputAttack()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ControlDelay = SwordAttackDelay;
            anim.SetTrigger("Attack");
        }
    }

    public void AttackAction()
    {
        Collider[] colliders = Physics.OverlapSphere(SwordAttackPosition.position, SwordAttackRange, EnemyLayers);
        Debug.Log("Colliders count " + colliders.Length);
        foreach (var enemyCollider in colliders)
        {
            DamageController.ApplyDamage(enemyCollider.gameObject, swordAttackDamage);
        }
    }

    bool IsGrounded()
    {
        Collider[] colliders = Physics.OverlapSphere(GroundCheck.position, k_GroundedRadius, GroundLayer);
        foreach (var c in colliders)
        {
            if (c.gameObject != gameObject)
                return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        if (GroundCheck == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GroundCheck.position, k_GroundedRadius);

        Handles.DrawWireDisc(SwordAttackPosition.position, Vector3.forward, SwordAttackRange);
    }

    private void FixedUpdate()
    {
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        Vector3 gravity = Vector3.up * (Physics.gravity.y * gravityScale);
        rigidbody.AddForce(gravity, ForceMode.Acceleration);
    }
}
