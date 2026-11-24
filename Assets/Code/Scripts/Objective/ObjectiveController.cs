using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectiveController : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject victory_zone;
    [SerializeField] private GameObject label;

    [SerializeField] private string unhacked_label;
    [SerializeField] private string hacked_label;

    [SerializeField] private bool indicator_visible = true;

    [Header("Audio")]
    [SerializeField] private AudioSource audio_source;
    [SerializeField] private AudioClip hack_sound;
    [SerializeField, Range(0f, 1f)] private float hack_sound_volume = 0.5f;

    private bool hacked = false;

    public bool OnInteract()
    {
        if (!hacked) {
            label.GetComponent<TextMeshProUGUI>().SetText(hacked_label);
            transform.Find("Indicator").GetComponent<ObjectiveIndicator>().gameObject.SetActive(false);
            if (audio_source != null && hack_sound != null) {
                audio_source.PlayOneShot(hack_sound, hack_sound_volume);
            }
            hacked = true;

            victory_zone.GetComponent<VictoryZone>().objectiveCompleted = true;

            return true;
        }
        else {
            return false;
        }
    }

    public void OnInteractTargetEnter()
    {
        label.SetActive(true);
    }

    public void OnInteractTargetExit()
    {
        label.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.Find("Indicator").GetComponent<ObjectiveIndicator>().gameObject.SetActive(indicator_visible);
        label.GetComponent<TextMeshProUGUI>().SetText(unhacked_label);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
