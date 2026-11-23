using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ECHO_HackParticle : MonoBehaviour
{
    [SerializeField] private float start_intensity = 5.0f;
    [SerializeField] private float end_intensitiy = 0.0f;
    [SerializeField] private float target_lifetime_ms = 1000;

    private float current_lifetime = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        current_lifetime += Time.deltaTime * 1000.0f;

        if (current_lifetime >= target_lifetime_ms) {
            Destroy(gameObject);
        }

        gameObject.transform.Find("Light_2D").gameObject.GetComponent<Light2D>().intensity = Mathf.Lerp(start_intensity, end_intensitiy, current_lifetime/target_lifetime_ms);
    }
}