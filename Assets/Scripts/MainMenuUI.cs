using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void PlayGame()
    {
        // Loads the next scene (LevelSelection)
        SceneManager.LoadScene("LevelSelection");
    }

    public void QuitGame()
    {
        // Works in a built game, won’t quit in editor
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
