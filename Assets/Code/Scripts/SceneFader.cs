using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private Image fadeOverlay;

    private static SceneFader instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RestartLevel()
    {
        StartCoroutine(Restart());
    }

    private IEnumerator Restart()
    {
        // haha u died
        yield return new WaitForSecondsRealtime(0.5f);

        // metallica reference
        yield return Fade(0f, 1f);

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return null; // here, take this extra frame 
        yield return Fade(1f, 0f);
    }

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        float duration = 0.75f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            Color color = fadeOverlay.color;
            // Math larp lol
            color.a = Mathf.Lerp(from, to, Mathf.Clamp01(elapsed / duration));
            fadeOverlay.color = color;

            yield return null;
        }

        Color finalColor = fadeOverlay.color;
        finalColor.a = to;
        fadeOverlay.color = finalColor;
    }
}