using UnityEngine;
using UnityEngine.Rendering;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private bool standGuard = false;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float turnDelay = 2f;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private float burstDelay = 0.01f;

    [SerializeField] private float waypointSize = 0.1f;

    [SerializeField] private Transform PathStart;
    [SerializeField] private Transform PathEnd;

    [SerializeField] private GameObject Bullet;
    [SerializeField] private Transform BulletOrigin;

    [SerializeField] private bool debugMode = true;

    private float defultFacingDirection;


    private Rigidbody2D rb;
    private EnemyAnimation enemyAnimation;

    private Vector2 PathStartPosition;
    private Vector2 PathEndPosition;
    private Vector2 currentTarget;
    private Vector2 patrolReturnTarget;
    private bool isHostile = false;
    private bool isReady = true;
    private bool isAlive = true;
    public enum enemyState
    {
        idle,
        patrol,
        look,
        investigate,
        chase,
        attack,
        interact,
        isHacked,
        dead,
        regular,
    }
    private enemyState defultState;
    private enemyState previousState;
    private enemyState currentState;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyAnimation = GetComponent<EnemyAnimation>();
        defultFacingDirection = transform.localScale.x;

        if (standGuard)
        {
            defultState = enemyState.idle;
            ChangeState(enemyState.idle);
            PathStartPosition = transform.position;
            return;
        }

        defultState = enemyState.patrol;
        if (PathEnd == null || PathStart == null)
        {
            Debug.LogError("Path Start or Path End was not found.");
            enabled = false;
            return;
        }

        PathEndPosition = PathEnd.position;
        PathStartPosition = PathStart.position;

        currentTarget = PathStartPosition;

        ChangeState(enemyState.patrol);
    }

    void Update()
    {
        if (!isAlive)
        {
            return;
        }

        switch (currentState)
        {
            case enemyState.idle:
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
                enemyAnimation.PlayIdle();
                break;
            case enemyState.patrol:
                enemyAnimation.PlayRun();
                Patrol();
                break;
            case enemyState.look:
                LookAtCurrentTarget();
                break;
            case enemyState.chase:
                // Chase logic would go here
                break;
            case enemyState.attack:
                if (!IsInvoking(nameof(Attack)))
                {
                    Invoke(nameof(Attack), attackDelay);
                }
                break;
            case enemyState.dead:
                rb.linearVelocity = Vector2.zero;
                enemyAnimation.PlayDeath();
                isAlive = false;
                break;
            case enemyState.investigate:
                GoToCurrentTarget();
                break;
            case enemyState.interact:
                // Interact logic would go here
                break;
            case enemyState.regular:
                previousState = currentState;
                ChangeState(defultState);
                break;
        }
    }

    public void ChangeState(enemyState newState)
    {
        currentState = newState;
        CancelInvoke();
    }

    void Patrol()
    {
        Vector2 direction =
       (currentTarget - rb.position).normalized;

        rb.linearVelocity = new Vector2(
            direction.x * speed,
            rb.linearVelocity.y
        );

        float stoppingDistance =
            waypointSize + speed * Time.fixedDeltaTime;

        if (Mathf.Abs(rb.position.x - currentTarget.x)
            <= stoppingDistance)
        {
            rb.linearVelocity = new Vector2(
                0f,
                rb.linearVelocity.y
            );

            ChangeState(enemyState.idle);

            Invoke(nameof(TurnAround), turnDelay);
        }
    }
    void GoToCurrentTarget()
    {
        LookAtCurrentTarget();
        Vector2 direction =
       (currentTarget - rb.position).normalized;

        rb.linearVelocity = new Vector2(
            direction.x * speed,
            rb.linearVelocity.y
        );

    }
    private void TurnAround()
    {
        if (currentState != enemyState.idle)
            return;

        if (currentTarget == PathStartPosition)
        {
            currentTarget = PathEndPosition;
        }
        else
        {
            currentTarget = PathStartPosition;
        }

        Vector2 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;

        ChangeState(enemyState.patrol);
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
    void Attack()
    {
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
        GameObject newBullet = Instantiate(Bullet, BulletOrigin.position, BulletOrigin.rotation);
        newBullet.SetActive(true);
        newBullet.GetComponent<BulletBehavior>().ShootTarget(currentTarget);
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
    void Investigate()
    {
        enemyAnimation.PlayRun();
        Vector2 direction =
       (currentTarget - rb.position).normalized;

        rb.linearVelocity = new Vector2(
            direction.x * speed,
            rb.linearVelocity.y
        );
        isHostile = true;
        enemyAnimation.isHostile(true);
        previousState = currentState;
        ChangeState(enemyState.attack);

    }
    public void BecomeHostile()
    {
        if (isHostile)
        {
            return;
        }
        isHostile = true;
        ChangeState(enemyState.chase);
    }

    public void StopPatrol()
    {
        CancelInvoke(nameof(TurnAround));
        rb.linearVelocity = Vector2.zero;
        enabled = false;
    }

    public void Die()
    {
        ChangeState(enemyState.dead);
    }
}
