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
    public float MagicAttackDelay;

    [Header("Jump")]
    public float JumpHeight;
    public float gravityScale;
    public float jumpPressTime;

    [Header("Other")]
    public float WalkSpeed;
    public Animator anim;
    public LayerMask EnemyLayers;
    public LayerMask GroundLayer;
    public GameUI gameUI;

    private Rigidbody rigidbody;
    private bool isJumping;
    private float jumpTimeCounter;
    private float controlDelay;

    Transform groundBelowCheck;
    const float groundCheckRadius = .2f;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        groundBelowCheck = transform.Find("GroundCheck");
    }

    void Update()
    {
        if (!CanMove())
        {
            controlDelay -= Time.deltaTime;

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
            controlDelay = MagicAttackDelay;
            anim.SetTrigger("Magic");
        }
    }

    public void FireballAction()
    {
        var fireballModel = new FireballModel
        {
            ownerTag = Tags.PLAYER,
            hitTag = Tags.ENEMY,
            originPosition = SwordAttackPosition.position,
            direction = transform.right,
            damage = 1,
            speed = 500f
        };

        FireballController.CreateFireball(fireballModel);
    }

    private bool CanMove()
    {
        return controlDelay <= 0;
    }

    private void InputMovement()
    {
        float x = Input.GetAxis("Horizontal");

        if (transform.parent == null)
        {
            rigidbody.MovePosition(rigidbody.position + new Vector3(x * WalkSpeed * Time.deltaTime, 0, 0));
        }
        else
        {
            // Move position doesnt work when the gameobject is child of another rigidbody
            // https://forum.unity.com/threads/child-rigidbody-doesnt-act-normally-when-parent-rigidbody-moves.564991/
            transform.position += new Vector3(x * WalkSpeed * Time.deltaTime, 0, 0);
        }

        if (x != 0)
        {
            transform.rotation = new Quaternion(0, x < 0 ? 180 : 0, 0, 0);
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
            controlDelay = SwordAttackDelay;
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
        Collider[] colliders = Physics.OverlapSphere(groundBelowCheck.position, groundCheckRadius, GroundLayer);
        foreach (var c in colliders)
        {
            if (c.gameObject != gameObject)
                return true;
        }

        return false;
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

    public void Death()
    {
        controlDelay = 4f;
        anim.SetTrigger("Dead");

        Invoke("ShowGameOverPanel", 3);
    }

    private void ShowGameOverPanel()
    {
        gameUI.ShowGameOverPanel();
    }

    public void PausePlayer(float duration)
    {
        controlDelay = duration;
        anim.SetFloat("Speed", 0);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (groundBelowCheck == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundBelowCheck.position, groundCheckRadius);

        Handles.DrawWireDisc(SwordAttackPosition.position, Vector3.forward, SwordAttackRange);
    }
#endif

}