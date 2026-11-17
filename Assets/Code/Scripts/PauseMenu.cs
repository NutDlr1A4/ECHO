using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuContent;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;

    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject settingsButton;
    [SerializeField] private GameObject masterVolumeSlider;

    public static bool IsPaused { get; private set; }

    private float previousTimeScale = 1f;

    private void Start()
    {
        IsPaused = false;

        pausePanel.SetActive(true);
        settingsPanel.SetActive(false);
        pauseMenuContent.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (!IsPaused)
                Pause();
            else if (settingsPanel.activeSelf)
                CloseSettings();
            else
                Resume();
        }
    }

    public void Pause()
    {
        if (IsPaused)
            return;

        previousTimeScale = Time.timeScale;

        IsPaused = true;
        Time.timeScale = 0f;

        pausePanel.SetActive(true);
        settingsPanel.SetActive(false);
        pauseMenuContent.SetActive(true);

        SelectUIElement(resumeButton);
    }

    public void Resume()
    {
        if (!IsPaused)
            return;

        IsPaused = false;
        Time.timeScale = previousTimeScale;

        EventSystem.current.SetSelectedGameObject(null);
        pauseMenuContent.SetActive(false);
    }

    public void OpenSettings()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);

        SelectUIElement(masterVolumeSlider);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);

        SelectUIElement(settingsButton);
    }

    private void SelectUIElement(GameObject element)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(element);
    }
}