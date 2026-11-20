using UnityEngine;
using static EnemyController;

public class EnemyAgro : MonoBehaviour
{
    [SerializeField] private float DetectionSpeed = 5f;
    [SerializeField] private float detectionTimeout = 2f;
    [SerializeField] private Transform detectionFill;
    private EnemyController enemyController;
    private Collider2D targetCollider = null;
    private Transform lastKnowLocation = null;
    private float detectionProgress = 0f;
    private float lastDetectionTime = 0f;
    private bool isDetected = false;
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
        if (detectionProgress >= 0 && Time.time - lastDetectionTime > detectionTimeout)
        {
            return;
        }

        if (lastKnowLocation == null) { return; }
        else if (targetCollider == null)
        { 
            UpdateDetectionProgress(-Time.deltaTime);
            return;
        }
        else if (lastKnowLocation != null)
        {
            DetectionCalculation();
        }
        else
        {
            Debug.LogWarning("Unknown state in EnemyAgro.Update.");
        }
    }
    public void updateSuspisionCollider2d(Collider2D newCollider)
    {
        if (newCollider == null)
        {
            targetCollider = null;
            return;
        }
        lastKnowLocation = newCollider.transform;
        lastDetectionTime = Time.time;
        targetCollider = newCollider;
        
    }
    private void DetectionCalculation()
    {
        float distance = Vector2.Distance(
            enemyController.transform.position,
            lastKnowLocation.position
        );
        distance += 0.001f;
        Debug.Log($"Distance to player: {distance}");
        Debug.Log($"Detection speed: {DetectionSpeed}");

        float distanceModifier = Mathf.Pow(DetectionSpeed / (distance * 0.5f), 2);

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
        if (detectionProgress <= 0.1f && isDetected)
        {
            Debug.Log("Enemy has lost track of the player!");
            enemyController.ChangeState(enemyState.regular);
            isDetected = false;
        }
        else if(detectionProgress <= 0.3f && !isDetected)
        {
            Debug.Log("Detection at 30%!");
            enemyController.LookPosition(lastKnowLocation.position);
        }
        else if (detectionProgress <= 0.6f && !isDetected)
        {
            Debug.Log("Detection at 60%!");
            enemyController.InvestigatePosition(lastKnowLocation.position);
        }
        else if(detectionProgress >= 1f && !isDetected)
        {
            Debug.Log("Enemy has fully detected the player!");
            isDetected = true;
            enemyController.ChangeState(enemyState.attack);
        }
    }
    
}
