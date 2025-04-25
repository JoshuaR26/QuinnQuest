using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public void RestartGame()
    {
        Time.timeScale = 1f; // Reset time scale in case it was paused
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            player.RestartGame();
        }
    }

    public void GoToMainMenu()
    {   
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync("MainMenu"); 
    }
}
