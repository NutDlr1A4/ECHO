using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectiveTerminal : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject victory_zone;
    [SerializeField] private GameObject label;

    [Header("Audio")]
    [SerializeField] private AudioSource audio_source;
    [SerializeField] private AudioClip hack_sound;

    private bool hacked = false;

    public bool OnInteract()
    {
        if (!hacked) {
            label.GetComponent<TextMeshProUGUI>().SetText("I'm in!");
            transform.Find("Indicator").GetComponent<ObjectiveIndicator>().gameObject.SetActive(false);
            audio_source.PlayOneShot(hack_sound);
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
