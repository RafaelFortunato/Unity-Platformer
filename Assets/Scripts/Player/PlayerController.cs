using UnityEditor;
using UnityEngine;
using System.Collections;

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

    [Header("GroundCheck")]
    public Transform groundBelowCheck;
    public LayerMask GroundLayer;

    [Header("Other")]
    public float WalkSpeed;
    public Animator anim;
    public LayerMask EnemyLayers;
    public GameUI gameUI;

    [Header("Sounds")]
    public AudioController audioController;
    public AudioSource swordA;
    public AudioSource swordB;
    public AudioSource fireballCreation;
    public AudioSource jumpLand;

    private Rigidbody rigidbody;
    private bool isJumping;
    private float longJumpTimeCounter;
    private float controlDelay;
    private float moveX;
    private bool isGrounded;

    const float groundCheckRadius = .2f;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
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

    private void FixedUpdate()
    {
        ApplyGravity();

        if (!CanMove())
        {
            if (!IsGrounded())
            {
                ApplyMovement();
            }

            return;
        }

        ApplyMovement();
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
            prefab = fireballPrefab,
            ownerTag = Tags.PLAYER,
            hitTag = Tags.ENEMY,
            originPosition = SwordAttackPosition.position,
            direction = transform.right,
            damage = 1
        };

        FireballController.CreateFireball(fireballModel);
        fireballCreation.Play();
    }

    private bool CanMove()
    {
        return controlDelay <= 0;
    }

    private void InputMovement()
    {
        moveX = Input.GetAxis("Horizontal");
    }

    void InputJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            isJumping = true;
            longJumpTimeCounter = jumpPressTime;
            rigidbody.velocity = Vector3.up * JumpHeight;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (longJumpTimeCounter > 0)
            {
                rigidbody.velocity = Vector2.up * JumpHeight;
                longJumpTimeCounter -= Time.deltaTime;
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
            (Random.Range(0, 2) == 0 ? swordA : swordB).Play();
        }
    }

    public void AttackAction()
    {
        Collider[] colliders = Physics.OverlapSphere(SwordAttackPosition.position, SwordAttackRange, EnemyLayers);
        // Debug.Log("Colliders count " + colliders.Length);
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
            {
                if (!isGrounded)
                {
                    jumpLand.Play();
                }

                isGrounded = true;
                return true;
            }
        }

        isGrounded = false;
        return false;
    }

    private void ApplyMovement()
    {
        transform.position += new Vector3(moveX * WalkSpeed * Time.deltaTime, 0, 0);

        if (moveX != 0)
        {
            transform.rotation = new Quaternion(0, moveX < 0 ? 180 : 0, 0, 0);
        }

        anim.SetFloat("Speed", Mathf.Abs(moveX));
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
        audioController.LowerBattleMusic(0.6f);
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