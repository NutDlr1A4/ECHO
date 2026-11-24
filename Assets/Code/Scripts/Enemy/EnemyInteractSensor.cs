using UnityEngine;

public class EnemyInteractSensor : MonoBehaviour, IInteractable
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

    void IInteractable.OnInteractTargetEnter()
    {
       
    }

    void IInteractable.OnInteractTargetExit()
    {
        
    }

    bool IInteractable.OnInteract()
    {
        Debug.Log("Enemy Interact death called");
        enemyController.Die();
        return true;
    }
}
