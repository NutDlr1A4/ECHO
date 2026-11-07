using UnityEngine;

public class EnemyDetectionCone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO: Add trigger event
            Debug.Log("Player entered the enemy's detection cone!");
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