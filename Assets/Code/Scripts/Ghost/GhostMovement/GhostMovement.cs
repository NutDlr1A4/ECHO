using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

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
    [SerializeField] private Light2D eyeLight;

    // SFX:
    [Header("Footstep Audio")]
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioClip footstepClip;

    [Header("Footstep Settings")]
    [SerializeField, Range(0f, 1f)] private float walkVolume = 0.5f;
    [SerializeField, Range(0f, 1f)] private float runVolume = 0.7f;
    [SerializeField] private float minPitch = 0.95f;
    [SerializeField] private float maxPitch = 1.05f;

    [Header("Crouch Audio")]
    [SerializeField] private AudioClip crouchClip;
    [SerializeField, Range(0f, 1f)] private float crouchVolume = 0.5f;

    [Header("Death Audio")]
    [SerializeField] private AudioClip deathHitClip;
    [SerializeField] private AudioClip deathFallClip;

    [SerializeField, Range(0f, 1f)] private float deathHitVolume = 0.7f;
    [SerializeField, Range(0f, 1f)] private float deathFallVolume = 0.6f;

    [Header("E.C.H.O Audio")]
    [SerializeField] private AudioClip echoEnterClip;
    [SerializeField] private AudioClip echoExitClip;

    [SerializeField, Range(0f, 1f)] private float echoVolume = 0.6f;

    [Header("Interaction Audio")]
    [SerializeField] private AudioClip interactClip;
    [SerializeField, Range(0f, 1f)] private float interactVolume = 0.4f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        eyeLight.enabled = true;
    }

    public void FinishInteraction()
    {
        isInteracting = false;
    }

    public void PlayFootstep()
    {
        if (isDead || isEchoMode || isCrouching || isInteracting)
            return;

        if (footstepSource == null || footstepClip == null)
            return;

        if (horizontalInput == 0f)
            return;

        footstepSource.pitch = Random.Range(minPitch, maxPitch);

        float volume = isRunning ? runVolume : walkVolume;

        footstepSource.PlayOneShot(footstepClip, volume);
    }

    public void PlayCrouchSound()
    {
        if (isDead || isEchoMode) return;
        if (footstepSource == null || crouchClip == null) return;

        footstepSource.pitch = Random.Range(0.95f, 1.05f);
        footstepSource.PlayOneShot(crouchClip, crouchVolume);
    }

    public void PlayDeathHit()
    {
        if (footstepSource == null || deathHitClip == null) return;

        footstepSource.pitch = 1f;
        footstepSource.PlayOneShot(deathHitClip, deathHitVolume);
    }

    public void PlayDeathFall()
    {
        if (footstepSource == null || deathFallClip == null) return;

        footstepSource.pitch = 1f;
        footstepSource.PlayOneShot(deathFallClip, deathFallVolume);
    }

    public void PlayInteractSound()
    {
        if (isDead || isEchoMode) return;
        if (footstepSource == null || interactClip == null) return;

        footstepSource.pitch = 1f;
        footstepSource.PlayOneShot(interactClip, interactVolume);
    }

    public void Die()
    {
        if (isDead)
            return;

        isDead = true;
        eyeLight.enabled = false;
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

        if (footstepSource != null && echoEnterClip != null)
        {
            footstepSource.pitch = 1f;
            footstepSource.PlayOneShot(echoEnterClip, echoVolume);
        }

        eyeLight.enabled = false;
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

        if (footstepSource != null && echoExitClip != null)
        {
            footstepSource.pitch = 1f;
            footstepSource.PlayOneShot(echoExitClip, echoVolume);
        }

        eyeLight.enabled = true;

        animator.SetBool("IsEchoMode", false);
    }

    void LateUpdate()
    {
        if (eyeLight != null)
        {
            Vector3 lightPosition = eyeLight.transform.localPosition;

            lightPosition.x = spriteRenderer.flipX
                ? -Mathf.Abs(lightPosition.x)
                : Mathf.Abs(lightPosition.x);

            eyeLight.transform.localPosition = lightPosition;
        }
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