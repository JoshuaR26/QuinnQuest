using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public void RestartGame()
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            player.RestartGame();
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu"); 
    }
}
