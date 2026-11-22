using UnityEngine;

public class EnemyDetectionCone : MonoBehaviour
{
<<<<<<< Updated upstream
    private void OnTriggerEnter2D(Collider2D other)
=======
    private Collider2D seenObject = null;
    private EnemyAgro enemyAgro;
    private bool canSeePlayer = false;
    private void Awake()
>>>>>>> Stashed changes
    {
        if (other.CompareTag("Player"))
        {
<<<<<<< Updated upstream
            // TODO: Add trigger event
            Debug.Log("Player entered the enemy's detection cone!");
=======
            Debug.LogError("EnemyAgro not found in parent.");
        }
    }

    private void Update()
    {
        enemyAgro.updateSuspisionCollider2d(seenObject);
        //if(seenObject != null)
        //{
        //    enemyAgro.updateSuspisionCollider2d(seenObject);
        //}
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
       
        RaycastHit2D[] hits = Physics2D.LinecastAll(
                transform.position,
                other.transform.position);
        
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.IsChildOf(transform.root))
            {
                continue;
            }
            if (hit.collider.CompareTag("Player"))
            {
                seenObject = other;
            }
            else
            {
                seenObject = null;
            }
            return;
>>>>>>> Stashed changes
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //TODO: Add trigger for deagro
            Debug.Log("Player left the enemy's detection cone!");
        }
    }
}