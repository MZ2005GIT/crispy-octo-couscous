using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void Resume()
    {
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync("PauseMenu"); // Unload additive scene
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Replace with your main menu scene
    }
}
