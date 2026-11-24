using UnityEngine;

public class EnemySoundSensorTest : MonoBehaviour
{
    private Collider2D seenObject;
    private EnemyAgro enemyAgro;
    private GhostMovement ghostMovement;
    private CircleCollider2D ownCollider2D;

    private void Awake()
    {
        enemyAgro =
            transform.root.GetComponentInChildren<EnemyAgro>(true);

        GameObject ghost =
            GameObject.FindGameObjectWithTag("Player");

        if (ghost != null)
        {
            ghostMovement =
                ghost.GetComponent<GhostMovement>();
        }

        ownCollider2D =
            GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        if (seenObject == null ||
        ghostMovement == null ||
        !ghostMovement.IsRunning())
        {
            return;
        }

        RaycastHit2D[] hits = Physics2D.LinecastAll(
            transform.position,
            seenObject.transform.position
        );

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.IsChildOf(transform.root))
            {
                continue;
            }

            if (hit.collider.CompareTag("Player"))
            {
                enemyAgro.SoundDetection(seenObject);
            }

            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            seenObject = other;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            seenObject = other;
            Debug.Log("Player entered the hearing range.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            seenObject = null;
            Debug.Log("Player left the hearing range.");
        }
    }
}