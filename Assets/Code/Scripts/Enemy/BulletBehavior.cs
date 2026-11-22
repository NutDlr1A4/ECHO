using System.Buffers;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int lifetime = 3;
    private Rigidbody2D rb;
    private Transform enemyRoot;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ShootTarget(Vector2 targetPosition, Transform owner)
    {
        enemyRoot = owner;
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.IsChildOf(enemyRoot))
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            GhostMovement player =
                other.GetComponentInParent<GhostMovement>();

            if (player != null)
            {
                player.Die();
            }
        }
        Destroy(gameObject);
    }
}