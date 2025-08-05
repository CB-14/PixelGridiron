using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public Button pauseButton;
    public Button resumeButton;
    public Button quitButton;

    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false); // hide on start

        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitToMainMenu);
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        pauseButton.gameObject.SetActive(false); // hide pause button
    }

    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }

    void QuitToMainMenu()
    {
        Time.timeScale = 1f; // unpause before switching scenes
        SceneManager.LoadScene("MainMenu"); // make sure this is the correct scene name
    }
}
