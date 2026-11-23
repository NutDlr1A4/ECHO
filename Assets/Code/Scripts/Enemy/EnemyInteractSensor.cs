using UnityEngine;

public class EnemyInteractSensor : MonoBehaviour
{
    private EnemyController enemyController;
    void Start()
    {
        enemyController =
            transform.root.GetComponentInChildren<EnemyController>(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            KYS();
        }
    }

    void KYS()
    {
        enemyController.Die();
    }
}
