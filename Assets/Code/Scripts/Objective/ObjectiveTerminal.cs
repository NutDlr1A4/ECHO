using TMPro;
using UnityEngine;

public class ObjectiveTerminal : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject label;

    private bool hacked = false;

    public bool OnInteract()
    {
        if (!hacked) {
            label.GetComponent<TextMeshProUGUI>().SetText("I'm in!");
            transform.Find("Indicator").GetComponent<ObjectiveIndicator>().gameObject.SetActive(false);

            // Set victory condition to true here!

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
