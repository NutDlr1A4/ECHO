using UnityEngine;
using UnityEngine.InputSystem;

public class GhostMovement : MonoBehaviour
{
    public float moveSpeed = 3f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider;

    private float horizontalInput;
    private bool isCrouching;

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

    void Update()
    {
        horizontalInput = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed)
                horizontalInput -= 1f;

            if (Keyboard.current.dKey.isPressed)
                horizontalInput += 1f;

            isCrouching = Keyboard.current.sKey.isPressed;

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
        }

        // Switch between idle and walking animations
        animator.SetBool("IsWalking", horizontalInput != 0f);
        animator.SetBool("IsCrouching", isCrouching);

        // Make Ghost face the direction he is moving
        if (horizontalInput < 0)
            spriteRenderer.flipX = true;
        else if (horizontalInput > 0)
            spriteRenderer.flipX = false;
    }

    void FixedUpdate()
    {
        if (isCrouching)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(
                horizontalInput * moveSpeed,
                rb.linearVelocity.y
            );
        }
    }
}