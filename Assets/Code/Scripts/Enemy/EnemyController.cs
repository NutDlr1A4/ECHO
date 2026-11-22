using UnityEngine;
using static EnemyController;

public class EnemyController : MonoBehaviour
{
       public enum enemyState
    {
        idle,
        patrol,
        chase,
        attack,
        dead
    }

    [SerializeField] private float speed = 5f;
    [SerializeField] private float turnDelay = 2f;
    [SerializeField] private float waypointSize = 0.1f;

<<<<<<< Updated upstream
    private Transform pointA;
    private Transform pointB;
    //private Transform facing;

=======
    [SerializeField] private Transform PathStart;
    [SerializeField] private Transform PathEnd;

    [SerializeField] private GameObject Bullet;
    [SerializeField] private Transform BulletOrigin;
    private float defultFacingDirection;
>>>>>>> Stashed changes
    private Rigidbody2D rb;
    private EnemyAnimation enemyAnimation;


    private Vector2 pointAPosition;
    private Vector2 pointBPosition;
    private Vector2 currentTarget;
<<<<<<< Updated upstream

=======
    private Vector2 patrolReturnTarget;
    private bool isHostile = false;
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
>>>>>>> Stashed changes
    private enemyState currentState;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyAnimation = GetComponent<EnemyAnimation>();


        pointB = transform.Find("Sub-entities/Path Start");
        pointA = transform.Find("Sub-entities/Path End");

        if (pointA == null || pointB == null)
        {
            Debug.LogError("Path Start or Path End was not found.");
            enabled = false;
            return;
        }

        pointAPosition = pointA.position;
        pointBPosition = pointB.position;

        currentTarget = pointBPosition;

        ChangeState(enemyState.patrol);
    }

    
    void Update()
    {
        switch(currentState)
        {
            case enemyState.idle:
                ChangeState(enemyState.idle);
                break;
            case enemyState.patrol:
                Patrol();
                break;
            case enemyState.chase:
                // Chase logic would go here
                break;
            case enemyState.attack:
                // Attack logic would go here
                break;
            case enemyState.dead:
                // Dead logic would go here
                break;
        }
    }

    public void ChangeState(enemyState newState)
    {
        currentState = newState;
<<<<<<< Updated upstream
=======
        CancelInvoke();
    }
    void StandStill()
    {

    }
>>>>>>> Stashed changes

        switch (currentState)
        {
            case enemyState.idle:
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
                enemyAnimation.PlayIdle();
                break;

            case enemyState.patrol:
                enemyAnimation.PlayRun();
                break;

            case enemyState.chase:
                enemyAnimation.PlayRun();
                break;

            case enemyState.attack:
                //Attack logic would go here
                break;

            case enemyState.dead:
                CancelInvoke(nameof(TurnAround));
                rb.linearVelocity = Vector2.zero;
                enemyAnimation.PlayDeath();
                break;
        }
    }
    void Patrol()
    {
        enemyAnimation.PlayRun();
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
    private void TurnAround()
    {
        if (currentState != enemyState.idle)
            return;

        if (currentTarget == pointBPosition)
        {
            currentTarget = pointAPosition;
        }
        else
        {
            currentTarget = pointBPosition;
        }

        Vector2 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;

        ChangeState(enemyState.patrol);
    }
<<<<<<< Updated upstream
=======
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
        newBullet.GetComponent<BulletBehavior>().ShootTarget(currentTarget,transform.parent);
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

>>>>>>> Stashed changes
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
