using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // This function will be called when the "Play Now" button is clicked
    public void PlayGame()
    {
        // Load the game scene. Replace "GameScene" with the name of your game scene
        SceneManager.LoadScene("Level 1");
    }

    // This function will be called when the "Quit" button is clicked
    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
        
        // If running in the Unity editor, stop play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
