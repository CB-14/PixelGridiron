using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadGameBoard()
    {
        Debug.Log("Loading Game Board...");
        SceneManager.LoadScene("Game Board");
    }
}