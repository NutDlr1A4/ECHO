using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float turnDelay = 2f;
    [SerializeField] private float waypointSize = 0.1f;

    public GameObject pointA;
    public GameObject pointB;

    private Rigidbody2D rb;

    private Animator animator;

    private Vector2 pointAPosition;
    private Vector2 pointBPosition;
    private Vector2 currentTarget;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("isRunning", true);

        pointAPosition = pointA.transform.position;
        pointBPosition = pointB.transform.position;
        currentTarget = pointBPosition;
        
    }

    private void Update()
    {
        Vector2 direction = (currentTarget - rb.position).normalized;

        rb.linearVelocity = direction * speed;

        float stoppingDistance = waypointSize + speed * Time.fixedDeltaTime;

        if (Vector2.Distance(rb.position, currentTarget) <= stoppingDistance)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isRunning", false);
            Invoke(nameof(TurnAround), turnDelay);
            enabled = false;
        }
    }

    private void TurnAround()
    {
        if (currentTarget == pointBPosition)
        {
            currentTarget = pointAPosition;
        }
        else
        {
            currentTarget = pointBPosition;
        }

        Flip();
        animator.SetBool("isRunning", true);
        enabled = true;
    }

    public void StopPatrol()
    {
        CancelInvoke(nameof(TurnAround));
        rb.linearVelocity = Vector2.zero;
        enabled = false;
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, waypointSize);
        Gizmos.DrawWireSphere(pointB.transform.position, waypointSize);
        Gizmos.DrawLine(pointB.transform.position, pointA.transform.position);
    }
}
