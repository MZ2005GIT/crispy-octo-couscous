using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void LoadLevel1()
    {
        SceneManager.LoadScene("lvl1");
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("lvl2");
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
