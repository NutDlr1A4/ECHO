using UnityEngine;

public class EnemyTouchSensor : MonoBehaviour
{
    private EnemyController enemyController;
    private Collider2D seenObject;
    private GameObject ghost;
    void Start()
    {
        enemyController =
            transform.root.GetComponentInChildren<EnemyController>(true);
        ghost = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") &&
            !enemyController.IsHostile())
        {
            enemyController.LookPosition(other.transform.position);
        }
        else if (other.transform.root != transform.root &&
            other.CompareTag("Enemy") &&
            !enemyController.IsHostile() &&
            !enemyController.Isinvestigateing())
        {
            enemyController.InvestigatePosition(ghost.transform.position);
        }
    }
}
