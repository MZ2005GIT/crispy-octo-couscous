using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private GameManager gameManager;
    public void LoadLevel1()
    {
        SceneManager.LoadScene("lvl1");
        //SceneManager.LoadScene("UI", LoadSceneMode.Additive);
    }

    public void LoadLevel2()
    {
        if (gameManager.levelUnlock == true)
        {
            SceneManager.LoadScene("lvl2");
            //SceneManager.LoadScene("UI", LoadSceneMode.Additive);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
