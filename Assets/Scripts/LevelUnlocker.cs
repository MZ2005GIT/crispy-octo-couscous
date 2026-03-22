using UnityEngine;
using UnityEngine.UI;

public class LevelUnlocker : MonoBehaviour
{
    public Button level1Button;
    public Button level2Button;
    public GameObject lockIconLevel2; // optional: if you have a lock image over the button
    private GameManager gameManager;

    void Start()
    {
        // Level 1 is always available
        level1Button.interactable = true;

        // Check if Level 1 has been cleared
        if (gameManager.levelUnlock == true)
        {
            // Unlock Level 2
            level2Button.interactable = true;
            Image lvl2img = level2Button.GetComponent<Image>();
            level2Button.GetComponent<Image>().color = new Color(lvl2img.color.r, lvl2img.color.g, lvl2img.color.b, 255);
            if (lockIconLevel2 != null) lockIconLevel2.SetActive(false);
        }
        else
        {
            // Keep Level 2 locked
            level2Button.interactable = false;
            if (lockIconLevel2 != null) lockIconLevel2.SetActive(true);
        }
    }
}
