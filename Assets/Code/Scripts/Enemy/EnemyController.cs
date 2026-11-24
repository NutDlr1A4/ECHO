using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class EnemyController : MonoBehaviour, IHackable
{
    [SerializeField] private bool standGuard = false;
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 5f;
    private float currentSpeed;
    [SerializeField] private float hackedDuration = 2;
    [SerializeField] private float hackCooldown = 5;
    private float lastHackedTime = 0;
    private bool isHacked = false;
    [SerializeField] private float turnDelay = 2f;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private float burstDelay = 0.01f;

    [SerializeField] private float waypointSize = 0.5f;

    [SerializeField] private Transform PathStart;
    [SerializeField] private Transform PathEnd;

    [SerializeField] private GameObject Bullet;
    [SerializeField] private Transform BulletOrigin;

    private Rigidbody2D rb;
    private EnemyAnimation enemyAnimation;
    private SpriteRenderer spriteRenderer;
    private EnemySFX enemySFX;

    private Vector2 PathStartPosition;
    private Vector2 PathEndPosition;
    private Vector2 currentTarget;
    private Vector2 patrolReturnTarget;

    private float defultFacingDirection;
    private float jumpForce = 0.5f;
    private float stuckTimer;

    private bool isHostile = false;
    private bool isinvestigateing = false;
    private bool isMoving = false;
    private bool isAlive = true;
    private bool deathCall = false;
    public enum enemyState
    {
        idle,
        patrol,
        guard,
        look,
        investigate,
        chase,
        attack,
        interact,
        isHacked,
        dead,
        victory,
        regular,
    }
    private enemyState defultState;
    private enemyState previousState;
    private enemyState currentState;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyAnimation = GetComponent<EnemyAnimation>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        enemySFX = GetComponentInChildren<EnemySFX>();

        currentSpeed = walkSpeed;

        if (PathEnd == null || PathStart == null)
        {
            Debug.LogError("Path Start or Path End was not found.");
            enabled = false;
            return;
        }
        currentTarget = PathStart.position;
        LookAtCurrentTarget();
        if (standGuard)
        {
            defultState = enemyState.guard;
            PathStartPosition = transform.position;
        }
        else
        {
            defultState = enemyState.patrol;
            PathEndPosition = PathEnd.position;
            PathStartPosition = PathStart.position;
        }
        currentTarget = PathStartPosition;
        defultFacingDirection = transform.localScale.x;
        ChangeState(defultState);
    }

    void Update()
    {
        if (!isAlive || isHacked)
        {
            return;
        }
        if (isMoving)
        {
            CheckIfStuck();
        }

        switch (currentState)
        {
            case enemyState.idle:
                StopWalk();
                break;
            case enemyState.patrol:
                if (!IsInvoking(nameof(ChangePatrolTarget)))
                {
                    Patrol();
                }
                break;
            case enemyState.guard:
                StandGuard();
                break;
            case enemyState.look:
                LookAtCurrentTarget();
                break;
            case enemyState.chase:
                ChaseBehavior();
                break;
            case enemyState.attack:
                if (!IsInvoking(nameof(Attack)))
                {
                    Invoke(nameof(Attack), attackDelay);
                }
                break;
            case enemyState.dead:
                DeathBehavior();
                break;
            case enemyState.investigate:
                GoToCurrentTarget();
                break;
            case enemyState.interact:
                // Interact logic would go here
                break;
            case enemyState.regular:
                previousState = currentState;
                UpdateCurrentTarget(PathStartPosition);
                GoToCurrentTarget();
                ChangeState(defultState);
                break;
            case enemyState.victory:
                if (!IsInvoking(nameof(VictoryDance))) {
                    Invoke(nameof(VictoryDance), 0.25f);
                }
                break;
        }
    }

    public void ChangeState(enemyState newState)
    {
        if (deathCall && isAlive)
        {
            currentState = enemyState.dead;
        }
        else if (currentState == enemyState.victory ||
            isHacked)
        {
            return;
        }
        else if (isAlive)
        {
            Debug.Log(newState);
            currentState = newState;
            CancelInvoke();
        }
    }
    void StandGuard()
    {
        float distance = Vector2.Distance(
            transform.position,
            currentTarget
        );
        float stoppingDistance =
            waypointSize + walkSpeed * Time.fixedDeltaTime;
        if (distance > stoppingDistance)
        {
            GoToCurrentTarget();
        }
        else if (transform.localScale.x !=
            defultFacingDirection)
        {
            Vector3 scale = transform.localScale;
            scale.x = defultFacingDirection;
            transform.localScale = scale;
        }
        else
        {
            StopWalk();
        }
    }
    void Patrol()
    {
        float distance = Vector2.Distance(
            transform.position,
            currentTarget
        );
        float stoppingDistance =
            waypointSize + walkSpeed * Time.fixedDeltaTime;
        if (distance > stoppingDistance)
        {
            GoToCurrentTarget();
        }
        else
        {
            StopWalk();
            Invoke(nameof(ChangePatrolTarget), turnDelay);
        }
    }
    void ChangePatrolTarget()
    {
        if (currentTarget == PathStartPosition)
        {
            currentTarget = PathEndPosition;
        }
        else
        {
            currentTarget = PathStartPosition;
        }
    }
    void StopWalk()
    {
        enemyAnimation.PlayIdle();
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        isMoving = false;
    }
    void GoToCurrentTarget()
    {
        isMoving = true;
        LookAtCurrentTarget();
        enemyAnimation.PlayRun();
        Vector2 direction =
       (currentTarget - rb.position).normalized;

        rb.linearVelocity = new Vector2(
            direction.x * currentSpeed,
            rb.linearVelocity.y
        );
    }

    void LookAtCurrentTarget()
    {
        Vector3 scale = transform.localScale;

        if (currentTarget.x > transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x);   // Face right
        }
        else
        {
            scale.x = -Mathf.Abs(scale.x);  // Face left
        }
        transform.localScale = scale;
    }
    void ChaseBehavior()
    {
        float distance = Vector2.Distance(
            transform.position,
            currentTarget);
        if (distance >= 12f)
        {
            GoToCurrentTarget();
        }
        else
        {
            StopWalk();
            if (!IsInvoking(nameof(Attack)))
            {
                Invoke(nameof(Attack), attackDelay);
            }
        }
    }
    void Attack()
    {
        if (!isAlive) { return; }
        StopWalk();
        for (int i = 0; i < 3; i++)
        {
            Invoke(nameof(Shoot), burstDelay * i);
            Debug.Log("PEW");
        }
    }

    void Shoot()
    {
        if (Bullet == null || BulletOrigin == null)
        {
            Debug.LogError("Bullet or BulletOrigin is not assigned.");
            return;
        }
        enemySFX.PlaySoundShoot();
        LookAtCurrentTarget();
        GameObject newBullet = Instantiate(Bullet, BulletOrigin.position, BulletOrigin.rotation);
        newBullet.SetActive(true);
        newBullet.GetComponent<BulletBehavior>().ShootTarget(currentTarget, transform.parent);
    }

    //Giga scuffed solution incoming, ALARM ALARM
    private int fallStep;
    private int fallSteps;
    private int fallRotates;
    private float fallHeight;
    void DeathBehavior()
    {
        if (!isAlive) { return; }
        isAlive = false;
        rb.linearVelocity = Vector2.zero;
        spriteRenderer.color = Color.gray;
        CancelInvoke();
        GameObject detectionBar = transform.Find("Visuals/DetectionBar").gameObject;
        detectionBar.SetActive(false);
        GameObject detectionFill = transform.Find("Visuals/DetectionFill").gameObject;
        detectionFill.SetActive(false);
        GameObject sensors = transform.Find("Sensors").gameObject;
        sensors.SetActive(false);
        GameObject attack = transform.Find("Attack").gameObject;
        attack.SetActive(false);

        CapsuleCollider2D hitbox = GetComponent<CapsuleCollider2D>();
        fallHeight = hitbox.bounds.size.y / 3f;
        rb.gravityScale = 0f;
        hitbox.enabled = false;
        enemySFX.PlaySoundDeath();
        enemyAnimation.PlayDeath();

        fallStep = 0;
        fallSteps = 10;
        fallRotates = 90 / fallSteps;
        fallHeight = fallHeight / fallSteps;

        DeathFall();
    }

    void DeathFall()
    {
        transform.position += Vector3.down * fallHeight;
        transform.Rotate(0f, 0f, fallRotates);

        fallStep++;

        if (fallStep < fallSteps)
        {
            Invoke(nameof(DeathFall), 0.05f);
        }
    }
    private bool danceFlip;
    void VictoryDance()
    {
        spriteRenderer.color = Color.gray;
        GameObject detectionBar = transform.Find("Visuals/DetectionBar").gameObject;
        detectionBar.SetActive(false);
        GameObject detectionFill = transform.Find("Visuals/DetectionFill").gameObject;
        detectionFill.SetActive(false);
        GameObject sensors = transform.Find("Sensors").gameObject;
        sensors.SetActive(false);
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        enemyAnimation.PlayRun();

        danceFlip = !danceFlip;

        if (danceFlip)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 15f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -15f);
        }

        if (Random.Range(0, 5) == 0)
        {
            transform.Rotate(0f, 0f, 360f);
        }

        spriteRenderer.color = new Color(
            Random.value,
            Random.value,
            Random.value
        );
    }
    public void VictoryTrigger()
    {
        Debug.Log("Enemy Victory");
        ChangeState(enemyState.victory);
    }

    public void InvestigatePosition(Vector2 position)
    {
        patrolReturnTarget = currentTarget;
        currentTarget = position;
        if (!isinvestigateing)
        {
            enemySFX.PlaySoundAlert();
            isinvestigateing = true;
        }
        ChangeState(enemyState.investigate);
    }
    public void LookPosition(Vector2 position)
    {
        patrolReturnTarget = currentTarget;
        currentTarget = position;
        StopWalk();
        ChangeState(enemyState.look);
    }
    public void UpdateCurrentTarget(Vector2 newTarget)
    {
        currentTarget = newTarget;
    }
    public void BecomeHostile()
    {
        if (isHostile)
        {
            Debug.Log("BecomeHostile() Failed");
            return;
        }
        Debug.Log("BecomeHostile() Sucsess");
        currentSpeed = runSpeed;
        isHostile = true;
        enemyAnimation.IsHostile(true);
        LookAtCurrentTarget();
        ChangeState(enemyState.chase);
    }
    public void LoseHostile()
    {
        if (!isHostile) { return; }
        currentSpeed = walkSpeed;
        isHostile = false;
        enemyAnimation.IsHostile(false);
        ChangeState(enemyState.regular);
    }
    public void LoseInvestigate()
    {
        isinvestigateing = false;
        ChangeState(enemyState.regular);

    }
    public bool IsHostile()
    {
        return isHostile;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public void Die()
    {
        if (isAlive)
        {
            Debug.Log("Death called");
            DeathBehavior();
        }
    }

    bool IHackable.OnHack()
    {
        if (Time.time - lastHackedTime < hackCooldown)
        {
            return false;
        }

        lastHackedTime = Time.time;
        HackedBehavior();
        return true;
    }

    private void HackedBehavior()
    {
        isHacked = true;

        CancelInvoke(nameof(EndHack));
        Invoke(nameof(EndHack), hackedDuration);
    }

    private void EndHack()
    {
        isHacked = false;
    }

    private void CheckIfStuck()
    {
        if (Mathf.Abs(rb.linearVelocity.x) < 0.1f)
        {
            stuckTimer += Time.fixedDeltaTime;
            if(currentState == enemyState.patrol)
            {
                StopWalk();
                Invoke(nameof(ChangePatrolTarget), turnDelay);
            }
            else if (stuckTimer >= 1f)
            {
                Debug.Log("UnStick jump triggerd");
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    jumpForce
                );

                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }
    }
}
