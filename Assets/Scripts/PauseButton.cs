using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButton : MonoBehaviour
{
    public string pauseMenuSceneName = "PauseMenu";

    public void OpenPauseMenu()
    {
        Time.timeScale = 0f; // Freeze gameplay
        SceneManager.LoadSceneAsync(pauseMenuSceneName, LoadSceneMode.Additive);
    }
}
