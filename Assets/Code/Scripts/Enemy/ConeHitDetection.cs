using System;
using UnityEngine;

public class EnemyDetectionCone : MonoBehaviour
{
    private Transform playerTransform = null;
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
        if(playerTransform != null)
        {
            enemyAgro.updateSuspision(playerTransform);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            //Debug.Log("Player entered the enemy's detection cone!");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = null;
            Debug.Log("Player left the enemy's detection cone!");
        }
    }
}