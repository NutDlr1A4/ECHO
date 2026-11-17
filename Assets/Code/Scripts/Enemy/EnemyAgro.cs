using UnityEngine;

public class EnemyAgro : MonoBehaviour
{
    [SerializeField] private float DetectionSpeed = 5f;
    [SerializeField] private float detectionTimeout = 2f;
    private float lastDetectionTime = 0f;

    [SerializeField] private Transform detectionFill;
    private float detectionProgress = 0f;
    private EnemyController enemyController;
    private Transform target = null;
    private bool isDetecting = false;
    private void Awake()
    {
        enemyController =
            transform.root.GetComponentInChildren<EnemyController>(true);
        if (enemyController == null)
        {
            Debug.LogError("EnemyController not found in parent.");
        }
    }

    private void Update()
    {
        if (target == null && detectionProgress == 0f) { return; }

        if (target == null)
        {
            UpdateDetectionProgress(-Time.deltaTime);
            return;
        }

        if (Time.time - lastDetectionTime > detectionTimeout)
        {
            target = null;
            isDetecting = false;
            Debug.Log("Detection timeout reached. Resetting target.");
            return;
        }

    }
    public void updateSuspision(Transform newtarget)
    {
        if (newtarget == null)
        {
            return;
        }
        lastDetectionTime = Time.time;
        target = newtarget;
        float distance = Vector2.Distance(
            enemyController.transform.position,
            target.position
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
        if(detectionProgress >= 1f && !isDetecting)
        {
            isDetecting = true;
            enemyController.InvestigatePosition(target.position);
        }
    }
}
