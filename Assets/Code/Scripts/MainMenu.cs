using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string firstLevel;
    [SerializeField] private GameObject mainButtons;
    [SerializeField] private GameObject settingsPanel;

    [SerializeField] private GameObject settingsButton;
    [SerializeField] private GameObject firstSettingsSelected;

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
}