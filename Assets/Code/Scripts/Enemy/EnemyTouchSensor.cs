using UnityEngine;

public class EnemyTouchSensor : MonoBehaviour
{
    private EnemyController enemyController;
    private Collider2D seenObject;
    void Start()
    {
        enemyController =
            transform.root.GetComponentInChildren<EnemyController>(true);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") &&
            !enemyController.IsHostile())
        {
            enemyController.LookPosition(other.transform.position);
        }
    }
}
