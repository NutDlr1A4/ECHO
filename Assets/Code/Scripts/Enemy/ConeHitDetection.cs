using System;
using UnityEngine;

public class EnemyDetectionCone : MonoBehaviour
{
    private Collider2D seenObject = null;
    private EnemyAgro enemyAgro;
    private void Awake()
    {
        enemyAgro = transform.root.GetComponentInChildren<EnemyAgro>(true);
        if (enemyAgro == null)
        {
            Debug.LogError("EnemyAgro not found in parent.");
        }
    }

    private void Update()
    {
        if(seenObject != null)
        {
            enemyAgro.updateSuspisionCollider2d(seenObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other == seenObject)
        {
            return;
        }
        else if (other.CompareTag("Player"))
        {
            seenObject = other;
            Debug.Log("Player detected in the enemy's detection cone!");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            seenObject = null;
            Debug.Log("Player left the enemy's detection cone!");
        }
    }
}