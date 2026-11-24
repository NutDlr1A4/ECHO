using UnityEngine;

public class EnemyInteractSensor : MonoBehaviour, IKillable
{
    private EnemyController enemyController;
    void Start()
    {
        enemyController =
            transform.root.GetComponentInChildren<EnemyController>(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      
    }

    void KYS()
    {
        enemyController.Die();
    }

    bool IKillable.OnKill()
    {
        Debug.Log("Enemy Interact death called");
        enemyController.Die();
        return true;
    }
}
