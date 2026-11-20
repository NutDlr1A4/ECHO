using UnityEngine;
using static EnemyController;

public class EnemyAgro : MonoBehaviour
{
    [SerializeField] private float DetectionSpeed = 5f;
    [SerializeField] private float detectionTimeout = 2f;
    private float lastDetectionTime = 0f;

    [SerializeField] private Transform detectionFill;
    private float detectionProgress = 0f;
    private EnemyController enemyController;
    private Collider2D targetCollider = null;
    private bool isDetecting = false;
    private void Awake()
    {
        enemyController =
            transform.root.GetComponentInChildren<EnemyController>(true);
        if (enemyController == null)
        {
            Debug.LogError("EnemyController not found in parent.");
        }
        detectionFill.localScale = new Vector3(
            0,
            1f,
            1f
        );
    }

    private void Update()
    {
        if (targetCollider == null && detectionProgress == 0f) { return; }

        if (targetCollider == null)
        {
            UpdateDetectionProgress(-Time.deltaTime);
            return;
        }

        if (Time.time - lastDetectionTime > detectionTimeout)
        {
            targetCollider = null;
            isDetecting = false;
            Debug.Log("Detection timeout reached. Resetting target.");
            return;
        }

    }
    public void updateSuspisionCollider2d(Collider2D newCollider)
    {
        if (newCollider == null)
        {
            return;
        }
        lastDetectionTime = Time.time;
        targetCollider = newCollider;
        float distance = Vector2.Distance(
            enemyController.transform.position,
            targetCollider.transform.position
        );
        float distanceModifier = DetectionSpeed / (distance + 0.1f);

        UpdateDetectionProgress(Time.deltaTime * distanceModifier);
    }
    private void UpdateDetectionProgress(float amount)
    {
        detectionProgress += amount;
        detectionProgress = Mathf.Clamp01(detectionProgress);
        detectionFill.localScale = new Vector3(
            detectionProgress,
            1f,
            1f
        );
        if (detectionProgress <= 0f && isDetecting)
        {
            Debug.Log("Enemy has lost track of the player!");
            enemyController.ChangeState(enemyState.regular);
            isDetecting = false;
        }
        else if(detectionProgress <= 0.3f && !isDetecting)
        {
            Debug.Log("Detection at 30%!");
            enemyController.LookPosition(targetCollider.transform.position);
        }
        else if (detectionProgress <= 0.6f && !isDetecting)
        {
            Debug.Log("Detection at 60%!");
            enemyController.InvestigatePosition(targetCollider.transform.position);
        }
        else if(detectionProgress >= 1f && !isDetecting)
        {
            Debug.Log("Enemy has fully detected the player!");
            isDetecting = true;
            enemyController.ChangeState(enemyState.attack);
        }
    }
    
}
