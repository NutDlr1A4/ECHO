using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string firstLevel;
    [SerializeField] private GameObject mainButtons;
    [SerializeField] private GameObject settingsPanel;

    [SerializeField] private GameObject settingsButton;
    [SerializeField] private GameObject firstSettingsSelected;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject controlsButton;
    [SerializeField] private GameObject controlsBackButton;

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (settingsPanel.activeSelf)
                CloseSettings();
            else if (controlsPanel.activeSelf)
                CloseControls();
        }
    }
    public void PlayGame()
    {
        SceneLoader.Instance.LoadScene(firstLevel);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        mainButtons.SetActive(false);
        settingsPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(firstSettingsSelected);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainButtons.SetActive(true);

        EventSystem.current.SetSelectedGameObject(settingsButton);
    }

    public void OpenControls()
    {
        mainButtons.SetActive(false);
        controlsPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(controlsBackButton);
    }

    public void CloseControls() 
    {
        controlsPanel.SetActive(false);
        mainButtons.SetActive(true);

        EventSystem.current.SetSelectedGameObject(controlsButton);
    }
}