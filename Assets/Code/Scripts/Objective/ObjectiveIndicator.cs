using Unity.Mathematics;
using UnityEngine;

public class ObjectiveIndicator : MonoBehaviour
{
    [Header("Floating Animation")]
    [SerializeField] private float float_offset = 1.0f;
    [SerializeField] private float float_rate = 1.0f;

    [Header("Objective Position")]
    [SerializeField] private float objectiveOffsetY = 1.5f;

    [Header("Off-screen Indicator")]
    [SerializeField] private Transform objective;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float rotationSmoothTime = 0.15f;

    private GameObject triangle;
    private Quaternion originalRotation;
    private float rotationVelocity;

    void Start()
    {
        triangle = transform.Find("triangle").gameObject;
        originalRotation = triangle.transform.localRotation;
    }

    void Update()
    {
        Vector3 screenPosition = mainCamera.WorldToViewportPoint(objective.position);

        bool outsideScreen =
            screenPosition.x < 0 ||
            screenPosition.x > 1 ||
            screenPosition.y < 0 ||
            screenPosition.y > 1;

        if (outsideScreen)
        {
            screenPosition.x = Mathf.Clamp(screenPosition.x, 0.05f, 0.95f);
            screenPosition.y = Mathf.Clamp(screenPosition.y, 0.05f, 0.95f);

            Vector3 worldPosition = mainCamera.ViewportToWorldPoint(
                new Vector3(
                    screenPosition.x,
                    screenPosition.y,
                    Mathf.Abs(mainCamera.transform.position.z - transform.position.z)
                )
            );

            worldPosition.z = transform.position.z;
            transform.position = worldPosition;
        }
        else
        {
            // Plasser indikatoren over objektivet
            transform.position = objective.position +
                                 new Vector3(0, objectiveOffsetY, 0);
        }

        // Flyteanimasjon
        triangle.transform.position =
            transform.position +
            new Vector3(
                0,
                (1f + math.sin(
                    Time.timeSinceLevelLoad *
                    math.PI *
                    float_rate
                ) * float_offset * 2f) / 2f,
                0
            );

        // Roter trekanten mot objektivet når det er utenfor kameraet
        if (outsideScreen)
        {
            Vector3 direction = objective.position - transform.position;

            float targetAngle = Mathf.Atan2(
                direction.y,
                direction.x
            ) * Mathf.Rad2Deg + 90f;

            float currentAngle = triangle.transform.eulerAngles.z;

            float smoothAngle = Mathf.SmoothDampAngle(
                currentAngle,
                targetAngle,
                ref rotationVelocity,
                rotationSmoothTime
            );

            triangle.transform.rotation = Quaternion.Euler(0, 0, smoothAngle);
        }
        else
        {
            triangle.transform.localRotation = originalRotation;
            rotationVelocity = 0f;
        }
    }
}