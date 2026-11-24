using Unity.Mathematics;
using UnityEngine;

public class ObjectiveIndicator : MonoBehaviour
{
    [SerializeField] private float float_offset = 1.0f;
    [SerializeField] private float float_rate = 1.0f;
    private GameObject triangle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        triangle = transform.Find("triangle").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        triangle.transform.position = transform.position + new Vector3(0, (1f + math.sin(Time.timeSinceLevelLoad * math.PI * float_rate) * float_offset * 2f)/2f, 0);
    }
}
