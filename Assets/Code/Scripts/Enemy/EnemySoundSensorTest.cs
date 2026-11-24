using UnityEngine;

public class EnemySoundSensorTest : MonoBehaviour
{
    private Collider2D seenObject;
    private EnemyAgro enemyAgro;
    private GhostMovement ghostMovement;
    private EnemyRaycastLogic raycastLogic;
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

        raycastLogic =
            GetComponentInParent<EnemyRaycastLogic>();

        ownCollider2D =
            GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        {
            if (seenObject == null ||
                ghostMovement == null ||
                !ghostMovement.IsRunning())
            {
                return;
            }

            if (raycastLogic.CanDetect(
                transform.position,
                seenObject))
            {
                enemyAgro.SoundDetection(seenObject);
            }
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