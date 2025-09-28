using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Scene")]
    public string gameSceneName = "GameScene1";// Set Game scene here

    [Header("UI")]
    public GameObject optionsPanel;

    public GameObject menuPanel;

    private void Start()
    {
        InputBindings.LoadDefaults();
        PlayerPrefs.GetFloat("masterVolume", 1f);
        if (optionsPanel != null) optionsPanel.SetActive(false);

        if (menuPanel != null) menuPanel.SetActive(true);
    }

    public void StartGame()
    {
        // Make sure correct game scene is assigned
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenOptions()
    {
        if (optionsPanel != null) optionsPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void CloseOptions()
    {
        if (optionsPanel != null) optionsPanel.SetActive(false);
        menuPanel.SetActive(true);  
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quit Requested");
    }
}

