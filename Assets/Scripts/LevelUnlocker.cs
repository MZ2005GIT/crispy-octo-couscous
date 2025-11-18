using UnityEngine;
using UnityEngine.UI;

public class LevelUnlocker : MonoBehaviour
{
    public Button level1Button;
    public Button level2Button;
    public GameObject lockIconLevel2; // optional: if you have a lock image over the button

    void Start()
    {
        // Level 1 is always available
        level1Button.interactable = true;

        // Check if Level 1 has been cleared
        if (PlayerPrefs.GetInt("Level1Cleared", 0) == 1)
        {
            // Unlock Level 2
            level2Button.interactable = true;
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
