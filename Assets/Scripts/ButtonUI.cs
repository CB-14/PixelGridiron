using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadGameBoard()
    {
        
{
    Debug.Log("Clicked button, trying to load Game Board...");
    SceneManager.LoadScene("Game Board");
}

        SceneManager.LoadScene("Game Board");
    }
}
