using UnityEditor.Rendering;
using UnityEngine;
using static EnemyController;

public class EnemyAgro : MonoBehaviour
{
    [SerializeField] private float DetectionSpeed = 1f;
    [SerializeField] private float detectionTimeout = 2f;
    [SerializeField] private Transform detectionFill;
    private EnemyController enemyController;
    private Collider2D targetCollider = null;
    private Transform lastKnowLocation = null;
    private float detectionProgress = 0f;
    private float lastDetectionTime = 0f;
    float visionConeLength;
    private bool isHostile = false;
    private void Awake()
    {
        if(DetectionSpeed == 0f)
        {
            DetectionSpeed = 0.00001f;
        }
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
        visionConeLength = GetComponentInChildren<PolygonCollider2D>().bounds.size.x;
    }

    private void Update()
    {
        if (lastKnowLocation == null) { return; }
        else if (targetCollider == null)
        { 
            if(Time.time - lastDetectionTime > detectionTimeout)
            {
                UpdateDetectionProgress(-Time.deltaTime);
            }   
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
        distance *= 1/visionConeLength;
        float distanceModifier = Mathf.Exp(-distance/DetectionSpeed);
        Debug.Log($"DistanceModifier speed: {distanceModifier}");
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
        if (detectionProgress == 0f)
        {
            Debug.Log("Enemy has lost track of the player!");
            isHostile = false;
            lastKnowLocation = null;
            enemyController.ChangeState(enemyState.regular);
        }
        else if(detectionProgress <= 0.5f && !isHostile)
        {
            Debug.Log("Detection at 30%, enemy is looking!");
            enemyController.LookPosition(lastKnowLocation.position);
        }
        else if (detectionProgress <= 0.99f && !isHostile)
        {
            Debug.Log("Detection at 60%, enemy is investigating!");
            enemyController.InvestigatePosition(lastKnowLocation.position);
        }
        else if(detectionProgress >= 1f && !isHostile)
        {
            Debug.Log("Enemy has fully detected the player, ennemy is hostile!");
            isHostile = true;
            enemyController.ChangeState(enemyState.attack);
        }
    }
    
}
