using UnityEngine;

public class EnemyAgro : MonoBehaviour
{
    [SerializeField] private float detectionSpeed = 1f;
    [SerializeField] private float detectionCrouchModifier = 0.5f;
    [SerializeField] private float detectionRunModifier = 1.5f;
    [SerializeField] private float detectionTimeout = 2f;
    [SerializeField] private Transform detectionFill;
    private EnemyController enemyController;
    private GhostMovement ghostMovement;
    private Transform lastKnowLocation = null;
    private float detectionProgress = 0f;
    private float lastDetectionTime = 0f;
    //float maxRange;
    [SerializeField] private PolygonCollider2D visionCone;
    [SerializeField] private CircleCollider2D hearingSphere;
    private float visionConeLength;
    private float hearingSphereLength;
    //private int detectionStage = 0;
    private bool isHostile = false;
    private bool isNormal = true;
    private bool sightPriority = false;
    private bool activelyDetecting = false;
    private void Awake()
    {
        visionConeLength = visionCone.bounds.size.x;
        hearingSphereLength = hearingSphere.bounds.size.x;

        if (detectionSpeed == 0f)
        {
            detectionSpeed = 0.00001f;
        }
        enemyController =
            transform.root.GetComponentInChildren<EnemyController>(true);
        if (enemyController == null)
        {
            Debug.LogError("EnemyController not found in parent.");
        }
        GameObject ghost = GameObject.FindGameObjectWithTag("Player");

        if (ghost != null)
        {
            ghostMovement = ghost.GetComponent<GhostMovement>();
        }
        detectionFill.localScale = new Vector3(0, 1f, 1f);
    }

    private void LateUpdate()
    {
        if (enemyController.IsHacked()) {
            activelyDetecting = false;
            sightPriority = false;
            return; 
        }
        if (!activelyDetecting &&
        Time.time - lastDetectionTime >= detectionTimeout)
        {
            UpdateDetectionProgress(-Time.deltaTime);
        }
        activelyDetecting = false;
        sightPriority = false;
    }

    public void VisionDetection(Collider2D target)
    {
        sightPriority = true;
        SubmitDetection(target,visionConeLength, 1f);
    }

    public void SoundDetection(Collider2D target)
    {
        if (!sightPriority)
        {
            SubmitDetection(target, hearingSphereLength, 0.5f);
        }
    }

    public void SubmitDetection(
        Collider2D target,
        float sensorLength,
        float strengthModifier)
    {
        if (target == null ||
            sensorLength <= 0 ||
            enemyController.IsHacked())
        {
            return;
        }

        if (isHostile)
        {
            enemyController.UpdateCurrentTarget(target.transform.position);
        }

        activelyDetecting = true;
        lastDetectionTime = Time.time;
        lastKnowLocation = target.transform;

        float distance = Vector2.Distance(
            enemyController.transform.position,
            target.transform.position);
        
        distance *= 1 / sensorLength;
        float strength = 
            Mathf.Exp(-Mathf.Pow(distance / detectionSpeed, 2f));


        strength *= strengthModifier;

        UpdateDetectionProgress(Time.deltaTime * strength);
    }

    private void UpdateDetectionProgress(float amount)
    {
        if(amount > 0)
        {
            if (ghostMovement.IsRunning())
            {
                amount *= detectionRunModifier;
            }
            else if (ghostMovement.IsCrouching())
            {
                amount *= detectionCrouchModifier;
            }
        }

       detectionProgress += amount;
        detectionProgress = Mathf.Clamp01(detectionProgress);
        detectionFill.localScale = new Vector3(
            detectionProgress,
            1f,
            1f
        );

        if (enemyController.IsHacked()) { return; }

        if (detectionProgress >= 1f && !isHostile)
        {
            Debug.Log("Enemy has fully detected the player, ennemy is hostile!");
            isHostile = true;
            isNormal = true;
            enemyController.BecomeHostile();
        }
        else if (detectionProgress >= 0.66f && !isHostile)
        {
            isNormal = true;
            Debug.Log("Detection, enemy is investigating!");
            enemyController.InvestigatePosition(lastKnowLocation.position);
        }
        else if (detectionProgress >= 0.33f &&
            !isHostile)
        {
            Debug.Log("Detection, enemy is looking!");
            isNormal = true;
            enemyController.LookPosition(lastKnowLocation.position);
        }
        else if (detectionProgress == 0f && isNormal)
        {
            Debug.Log("Enemy has lost track of the player!");
            isHostile = false;
            isNormal = false;
            lastKnowLocation = null;
            enemyController.LoseHostile();
            enemyController.LoseInvestigate();
        }
    }
    public void BecomeHostile()
    {
        isHostile = true;
        UpdateDetectionProgress(10);
    }
}
