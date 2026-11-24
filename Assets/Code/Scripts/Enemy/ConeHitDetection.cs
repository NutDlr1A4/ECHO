using UnityEngine;

public class EnemyDetectionCone : MonoBehaviour
{
    private Collider2D seenObject;
    private EnemyAgro enemyAgro;
    private EnemyRaycastLogic raycastLogic;
    PolygonCollider2D ownCollider2D;

    private void Awake()
    {
        enemyAgro =
            transform.root.GetComponentInChildren<EnemyAgro>(true);

        if (enemyAgro == null)
        {
            Debug.LogError(
                "EnemyAgro was not found in the enemy hierarchy.",
                gameObject
            );

            enabled = false;
        }
        raycastLogic =
            GetComponentInParent<EnemyRaycastLogic>();

        if (raycastLogic == null)
        {
            Debug.LogError(
                "EnemyRaycastLogic was not found.",
                gameObject
            );

            ownCollider2D = GetComponent<PolygonCollider2D>();
        }
    }

    private void Update()
    {
        if (seenObject != null)
        {
            enemyAgro.VisionDetection(seenObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the detection cone.");
        }
    }

    //private void OnTriggerStay2D(Collider2D other)
    //{
    //    if (!other.CompareTag("Player"))
    //    {
    //        return;
    //    }

    //    RaycastHit2D[] hits = Physics2D.LinecastAll(
    //        transform.position,
    //        other.transform.position
    //    );

    //    foreach (RaycastHit2D hit in hits)
    //    {
    //        if (hit.transform.IsChildOf(transform.root))
    //        {
    //            continue;
    //        }

    //        if (hit.collider.CompareTag("Player"))
    //        {
    //            seenObject = other;
    //        }
    //        else
    //        {
    //            seenObject = null;
    //        }

    //        return;
    //    }

    //    seenObject = null;
    //}

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (raycastLogic.CanDetect(
            transform.position,
            other))
        {
            seenObject = other;
        }
        else
        {
            seenObject = null;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            seenObject = null;

            Debug.Log("Player left the detection cone.");
        }
    }
}