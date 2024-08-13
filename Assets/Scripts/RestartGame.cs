using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public void RestartGame() 
    {
        // Reset the time scale to ensure the game runs at normal speed
        Time.timeScale = 1f; 

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the game
    }
}

