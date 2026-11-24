using UnityEngine;

public class EnemySoundSensor : MonoBehaviour
{
    private EnemyController enemyController;
    private GhostMovement ghostMovement;
    void Awake()
    {
        enemyController =
            transform.root.GetComponentInChildren<EnemyController>(true);
        
        GameObject ghost = GameObject.FindGameObjectWithTag("Player");
        if (ghost != null)
        {
            ghostMovement = ghost.GetComponent<GhostMovement>();
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") &&
            !enemyController.IsHostile() &&
            ghostMovement.IsRunning())
        {
            enemyController.LookPosition(other.transform.position);
        }
    }
}
