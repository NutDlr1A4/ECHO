using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;

    private Rigidbody2D rb;
    private Transform enemyRoot;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ShootTarget(Vector2 targetPosition, Transform owner)
    {
        enemyRoot = owner;

        Vector2 direction =
            (targetPosition - rb.position).normalized;

        rb.linearVelocity = direction * speed;

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (enemyRoot != null &&
            other.transform.IsChildOf(enemyRoot))
        {
            return;
        }

        if (other.transform.root.CompareTag("Enemy") ||
            other.transform.gameObject.layer ==
            LayerMask.NameToLayer("Ignore Raycast"))
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
                EnemyController enemyController =
                    enemyRoot.GetComponentInChildren<EnemyController>(true);

                if (enemyController != null)
                {
                    enemyController.VictoryTrigger();
                }
                else
                {
                    Debug.Log("No dance :-(");
                }
            }
        }

        Destroy(gameObject);
    }
}