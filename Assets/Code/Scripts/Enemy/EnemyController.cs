using UnityEngine;
using UnityEngine.Rendering;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private bool standGuard = false;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float turnDelay = 2f;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private float burstDelay = 0.01f;

    [SerializeField] private float waypointSize = 0.5f;

    [SerializeField] private Transform PathStart;
    [SerializeField] private Transform PathEnd;

    [SerializeField] private GameObject Bullet;
    [SerializeField] private Transform BulletOrigin;

    //[SerializeField] private bool debugMode = true;

    private float defultFacingDirection;


    private Rigidbody2D rb;
    private EnemyAnimation enemyAnimation;
    private SpriteRenderer spriteRenderer;

    private Vector2 PathStartPosition;
    private Vector2 PathEndPosition;
    private Vector2 currentTarget;
    private Vector2 patrolReturnTarget;

    private float jumpForce = 0.5f;
    private float stuckTimer;

    private bool isHostile = false;
    private bool isMoving = false;
    private bool isAlive = true;
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
            if (PathEnd == null || PathStart == null)
            {
                Debug.LogError("Path Start or Path End was not found.");
                enabled = false;
                return;
            }
            PathEndPosition = PathEnd.position;
            PathStartPosition = PathStart.position;
        }
        currentTarget = PathStartPosition;
        LookAtCurrentTarget();
        defultFacingDirection = transform.localScale.x;
        ChangeState(defultState);
    }

    void Update()
    {
        if (!isAlive)
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
                if (!IsInvoking(nameof(VictoryDance))){
                    Invoke(nameof(VictoryDance), 0.25f);
                }
                break;
        }
    }

    public void ChangeState(enemyState newState)
    {
        if (isAlive)
        {
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
            waypointSize + speed * Time.fixedDeltaTime;
        if (distance > stoppingDistance)
        {
            GoToCurrentTarget();
        }
        else if(transform.localScale.x !=
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
            waypointSize + speed * Time.fixedDeltaTime;
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
            direction.x * speed,
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
        else if (!IsInvoking(nameof(Attack)))
        {
            Invoke(nameof(Attack), attackDelay);
        }
    }
    void Attack()
    {
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
        isAlive = false;
        rb.linearVelocity = Vector2.zero;
        spriteRenderer.color = Color.gray;
        GameObject detectionBar = transform.Find("Visuals/DetectionBar").gameObject;
        GameObject detectionFill = transform.Find("Visuals/DetectionFill").gameObject;
        detectionBar.SetActive(false);
        detectionFill.SetActive(false);

        //BoxCollider2D hitbox = GetComponent<BoxCollider2D>();
        CapsuleCollider2D hitbox = GetComponent<CapsuleCollider2D>();
        fallHeight = hitbox.bounds.size.y / 2f;
        rb.gravityScale = 0f;
        hitbox.enabled = false;
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
        GameObject detectionFill = transform.Find("Visuals/DetectionFill").gameObject;
        detectionBar.SetActive(false);
        detectionFill.SetActive(false);
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
        ChangeState(enemyState.investigate);
    }
    public void LookPosition(Vector2 position)
    {
        patrolReturnTarget = currentTarget;
        currentTarget = position;
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
        isHostile = true;
        LookAtCurrentTarget();
        ChangeState(enemyState.chase);
    }
    public void LoseHostile()
    {
        if (!isHostile)
        {
            return;
        }
        isHostile = false;
        ChangeState(enemyState.regular);
    }
    public bool IsHostile()
    {
        return isHostile;
    }


    public void Die()
    {
        ChangeState(enemyState.dead);
    }

    private void CheckIfStuck()
    {
        if (Mathf.Abs(rb.linearVelocity.x) < 0.1f)
        {
            stuckTimer += Time.fixedDeltaTime;

            if (stuckTimer >= 1f)
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
