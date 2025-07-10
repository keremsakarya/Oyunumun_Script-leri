using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    bool isFacingRight = true;

    //? Audio
    public AudioClip attackSfx;
    public AudioClip footstepSfx;
    public AudioClip jumpSfx;

    private AudioSource audioSource;
    private bool isWalking = false;


    //? Movement
    [SerializeField] private float baseMoveSpeed = 8f;
    private float currentMoveSpeed;

    public float MoveSpeed => currentMoveSpeed;

    float horizontalMovement;

    //? Jumping
    [SerializeField] public float jumpPower = 10f;
    public int maxJumps = 2;
    int jumpsRemaining;

    //? Ground Check
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;
    bool isGrounded = false;

    //? Gravity
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultipler = 2f;

    //? Input System
    private PlayerInput playerInput;
    private InputAction attackAction;

    //? Attack
    [HideInInspector] public bool isAttacking = false;


    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        attackAction = playerInput.actions["Attack"];
        attackAction.performed += Attack;
    }

    void OnDisable()
    {
        attackAction.performed -= Attack;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = footstepSfx;

        currentMoveSpeed = baseMoveSpeed;
    }

    void Update()
    {
        rb.velocity = new Vector2(horizontalMovement * MoveSpeed, rb.velocity.y);
        GroundCheck();
        Gravity();
        Flip();

        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetFloat("magnitude", rb.velocity.magnitude);

        HandleFootstepSound();
    }

    private void HandleFootstepSound()
    {
        bool isCurrentlyWalking = animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") &&
                                  Mathf.Abs(horizontalMovement) > 0.1f &&
                                  isGrounded;

        if (isCurrentlyWalking && !isWalking)
        {
            isWalking = true;
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else if (!isCurrentlyWalking && isWalking)
        {
            isWalking = false;
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }


    public void Gravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultipler;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsRemaining > 0)
        {
            if (context.performed)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                jumpsRemaining--;
                animator.SetTrigger("Jump");

                if (jumpSfx != null)
                {
                    audioSource.PlayOneShot(jumpSfx);
                }
            }
            else if (context.canceled)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                jumpsRemaining--;
                animator.SetTrigger("Jump");

                if (jumpSfx != null)
                {
                    audioSource.PlayOneShot(jumpSfx);
                }
            }
        }
    }

    private void GroundCheck()
    {
        isGrounded = Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer);
        if (isGrounded)
        {
            jumpsRemaining = maxJumps;
        }
    }

    private void Flip()
    {
        if (isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }

    //? Attack input handler
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            bool isIdleOrWalking = animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") ||
                                   animator.GetCurrentAnimatorStateInfo(0).IsName("Walk");

            if (isIdleOrWalking)
            {
                animator.SetTrigger("attack");
                GetComponent<PlayerAttackHandler>().PerformAttack();

                //? attack gerçekleşiyor
                isAttacking = true;

                if (attackSfx != null)
                {
                    audioSource.PlayOneShot(attackSfx);
                }

                StartCoroutine(ResetAttackFlag());
            }
        }
    }

    private IEnumerator ResetAttackFlag()
    {
        yield return new WaitForSeconds(0.2f); // animasyon süresi kadar
        isAttacking = false;
    }

    private IEnumerator PlayFootsteps()
    {
        while (true)
        {
            if (footstepSfx != null)
            {
                audioSource.PlayOneShot(footstepSfx);
            }
            yield return new WaitForSeconds(0.4f); //* Yürüme adım süresi (animasyona göre ayarla)
        }
    }

    //? Boost kısmı
    public void ApplySpeedBoost(float multiplier, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(SpeedBoostRoutine(multiplier, duration));
    }

    private IEnumerator SpeedBoostRoutine(float multiplier, float duration)
    {
        currentMoveSpeed = baseMoveSpeed * multiplier;
        yield return new WaitForSeconds(duration);
        currentMoveSpeed = baseMoveSpeed;
    }
}
