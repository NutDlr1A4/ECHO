using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int lifetime = 3;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ShootTarget(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<GhostMovement>().Die();
            Destroy(gameObject);
        }
    }
}