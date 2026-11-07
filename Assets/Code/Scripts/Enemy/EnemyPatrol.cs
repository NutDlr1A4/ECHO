using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] public float speed = 5f;
    [SerializeField] public float waypointSize = 0.1f;
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Animator anim;
    //private Transform currentPoint;
    private Vector2 pointAPos;
    private Vector2 pointBPos;
    private Vector2 currentTarget;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        pointAPos = pointA.transform.position;
        pointBPos = pointB.transform.position;
        currentTarget = pointBPos;
        anim.SetBool("isRunning", true);
    }

    private void Update()
    {
        Vector2 position = rb.position;
        //Vector2 direction = currentTarget.position - transform.position;
        Vector2 direction = (currentTarget - position).normalized;

        direction = direction.normalized;
        rb.linearVelocity = direction * speed;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(direction.x);

        if (Vector2.Distance(transform.position, currentTarget.position) < waypointSize)
        {
            if (currentTarget == pointB.transform)
            {
                currentTarget = pointA.transform;
                Debug.Log("Current target is point A");
            }
            else
            {
                currentTarget = pointB.transform;
                Debug.Log("Current target is point B");
            }
            Flip();
        }
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
