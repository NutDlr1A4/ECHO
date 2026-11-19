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
        dead,
        investigate,
        interact,
        isHacked,
    }

    [SerializeField] private bool standGuard = false;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float turnDelay = 2f;
    [SerializeField] private float waypointSize = 0.1f;

    [SerializeField] private Transform PathStart;
    [SerializeField] private Transform PathEnd;

    [SerializeField] private GameObject Bullet;
    [SerializeField] private Transform BulletOrigin;

    [SerializeField] private bool debugMode = true;

    private Rigidbody2D rb;
    private EnemyAnimation enemyAnimation;

    private Vector2 PathStartPosition;
    private Vector2 PathEndPosition;
    private Vector2 currentTarget;
    private Vector2 patrolReturnTarget;

    private enemyState currentState;
    private bool isAgressive = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyAnimation = GetComponent<EnemyAnimation>();

        if (standGuard)
        {
            ChangeState(enemyState.idle);
            return;
        }


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
        switch(currentState)
        {
            case enemyState.idle:
                ChangeState(enemyState.idle);
                break;
            case enemyState.patrol:
                ChangeState(enemyState.patrol);
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
            case enemyState.investigate:
                // Investigate logic would go here
                ChangeState(enemyState.investigate);
                break;
            case enemyState.interact:
                // Interact logic would go here
                break;
        }
    }

    public void ChangeState(enemyState newState)
    {
        currentState = newState;

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
            case enemyState.investigate:
                //Investigate logic would go here
                Investigate();
                break;
            case enemyState.interact:
                //Interact logic would go here
                break;
        }
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
    void Attack()
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
    void Investigate()
    {
        enemyAnimation.PlayRun();
        Vector2 direction =
       (currentTarget - rb.position).normalized;

        rb.linearVelocity = new Vector2(
            direction.x * speed,
            rb.linearVelocity.y
        );
        isAgressive = true;
        enemyAnimation.isHostile(true);
        Attack();
        
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
