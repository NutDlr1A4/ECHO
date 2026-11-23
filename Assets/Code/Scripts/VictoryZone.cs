
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryZone : MonoBehaviour
{
    public bool objectiveCompleted = false;

    public string nextArea;

    private GameObject Indicator;

    private void Start()
    {
        Indicator = transform.Find("Indicator").gameObject;
        Indicator.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && objectiveCompleted)
        {
            SceneManager.LoadScene(nextArea);
        }
    }

    private void Update()
    {
        if (objectiveCompleted)
        {
            Indicator.SetActive(true);
        }
    }
}
