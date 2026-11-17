using UnityEngine;

public class EnemyDetectionCone : MonoBehaviour
{
    private EnemyController enemyController;
    private void Awake()
    {
        enemyController =
            transform.root.GetComponentInChildren<EnemyController>(true);
        if (enemyController == null)
        {
            Debug.LogError("EnemyController not found in parent.");
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO: Add trigger event
            Debug.Log("Player entered the enemy's detection cone!");
            enemyController.InvestigatePosition(other.transform.position);
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