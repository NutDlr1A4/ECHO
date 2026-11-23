using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class VictoryZone : MonoBehaviour
{
    public bool objectiveCompleted = false;

#if UNITY_EDITOR
    public SceneAsset nextArea;
#endif

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
#if UNITY_EDITOR
            SceneManager.LoadScene(nextArea.name);
#endif
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