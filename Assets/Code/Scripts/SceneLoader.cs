using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image fadeOverlay;
    [SerializeField] private GameObject loadingBackground;
    [SerializeField] private GameObject echoLogo;

    public static SceneLoader Instance { get; private set; }
    private bool isLoading;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        fadeOverlay.color = new Color(0f, 0f, 0f, 0f);
    }

    public void LoadScene(string sceneName)
    {
        if (!isLoading)
            StartCoroutine(Load(sceneName));
    }

    private IEnumerator Load(string sceneName)
    {
        isLoading = true;
        canvasGroup.blocksRaycasts = true;

        // sick fade
        loadingBackground.SetActive(false);
        echoLogo.SetActive(false);

        canvasGroup.alpha = 1f;
        yield return Fade(0f, 1f);

        // show city
        loadingBackground.SetActive(true);
        echoLogo.SetActive(true);

        yield return Fade(1f, 0f);

        // load next scene
        float startTime = Time.realtimeSinceStartup;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f ||
               Time.realtimeSinceStartup - startTime < 1.5f)
        {
            yield return null;
        }

        // hides loading screen
        yield return Fade(0f, 1f);

        operation.allowSceneActivation = true;

        while (!operation.isDone)
            yield return null;

        Time.timeScale = 1f;

        loadingBackground.SetActive(false);
        echoLogo.SetActive(false);

        // new scene reveal (GEM)
        yield return Fade(1f, 0f);

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        isLoading = false;
    }

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;

        while (elapsed < 0.5f)
        {
            elapsed += Time.unscaledDeltaTime;

            Color color = fadeOverlay.color;
            color.a = Mathf.Lerp(from, to, Mathf.Clamp01(elapsed / 0.5f));
            fadeOverlay.color = color;

            yield return null;
        }

        Color finalColor = fadeOverlay.color;
        finalColor.a = to;
        fadeOverlay.color = finalColor;
    }
}