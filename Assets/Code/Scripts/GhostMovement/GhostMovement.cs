using UnityEngine;
using UnityEngine.InputSystem;

public class GhostMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float runSpeed = 5f;
    private float horizontalInput;
    private bool isCrouching;
    private bool isRunning;
    private bool isInteracting;
    private bool isDead;
    private bool isEchoMode;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider;
    private Vector2 standingSize = new Vector2(0.89f, 1.83f);
    private Vector2 standingOffset = new Vector2(0f, -0.08f);
    private Vector2 crouchingSize = new Vector2(1.05f, 1.30f);
    private Vector2 crouchingOffset = new Vector2(0f, -0.345f);

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    public void FinishInteraction()
    {
        isInteracting = false;
    }

    public void Die()
    {
        if (isDead)
            return;

        isDead = true;
        isInteracting = false;
        isEchoMode = false;

        horizontalInput = 0f;
        isRunning = false;
        isCrouching = false;

        rb.linearVelocity = Vector2.zero;

        animator.ResetTrigger("Interact");
        animator.SetBool("IsEchoMode", false);
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsCrouching", false);
        animator.SetTrigger("Die");
    }

    public void EnterEchoMode()
    {
        if (isDead || isEchoMode || isCrouching || isInteracting)
            return;

        isEchoMode = true;
        isInteracting = false;

        horizontalInput = 0f;
        isRunning = false;
        isCrouching = false;

        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        capsuleCollider.size = standingSize;
        capsuleCollider.offset = standingOffset;

        animator.ResetTrigger("Interact");
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsCrouching", false);

        animator.SetBool("IsEchoMode", true);
    }

    public void ExitEchoMode()
    {
        if (!isEchoMode)
            return;

        isEchoMode = false;

        animator.SetBool("IsEchoMode", false);
    }

    void Update()
    {
        if (isDead)
            return;

        if (isEchoMode)
        {
            if (Keyboard.current != null &&
                Keyboard.current.eKey.wasPressedThisFrame)
            {
                ExitEchoMode();
            }

            return;
        }

        horizontalInput = 0f;
        isRunning = false;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.kKey.wasPressedThisFrame)
            {
                Die();
                return;
            }
            
            isCrouching = Keyboard.current.sKey.isPressed;

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                EnterEchoMode();
                return;
            }

            if (Keyboard.current.aKey.isPressed)
                horizontalInput -= 1f;

            if (Keyboard.current.dKey.isPressed)
                horizontalInput += 1f;

            isRunning = Keyboard.current.leftShiftKey.isPressed
            && horizontalInput != 0f
            && !isCrouching;

            if (isCrouching)
            {
                capsuleCollider.size = crouchingSize;
                capsuleCollider.offset = crouchingOffset;
            }
            else
            {
                capsuleCollider.size = standingSize;
                capsuleCollider.offset = standingOffset;
            }

            if (Keyboard.current.spaceKey.wasPressedThisFrame
            && !isCrouching
            && !isInteracting)
            {
                isInteracting = true;
                animator.SetTrigger("Interact");
            }
        }

        // Update Animator parameters based on player input
        animator.SetBool("IsWalking", horizontalInput != 0f && !isInteracting);
        animator.SetBool("IsCrouching", isCrouching && !isInteracting);
        animator.SetBool("IsRunning", isRunning && !isInteracting);

        // Make Ghost face the direction he is moving
        if (!isInteracting)
        {
            if (horizontalInput < 0)
                spriteRenderer.flipX = true;
            else if (horizontalInput > 0)
                spriteRenderer.flipX = false;
        }
    }

    void FixedUpdate()
    {

        if (isDead || isEchoMode)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        if (isCrouching || isInteracting)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
        else
        {
            float currentSpeed = isRunning ? runSpeed : moveSpeed;
            rb.linearVelocity = new Vector2(
                horizontalInput * currentSpeed,
                rb.linearVelocity.y
            );
        }
    }
}