using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private bool isOpened = false;
    private GameObject player;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isOpened) return;

        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            PlayerInventoryManager playerInventory = other.GetComponent<PlayerInventoryManager>();
            if (playerInventory != null)
            {
                int keysToConsume = playerInventory.currentKeys;
                if (keysToConsume >= 4)
                {
                    playerInventory.UseKeys(keysToConsume);

                    isOpened = true;
                    Debug.Log("Door opened! All keys consumed.");

                    // Mark the level as cleared (e.g., for Level 1)
                    string currentLevel = SceneManager.GetActiveScene().name;
                    if (currentLevel == "lvl1") // Adjust the scene name as needed
                    {
                        PlayerPrefs.SetInt("Level1Cleared", 1);
                        PlayerPrefs.Save(); // Ensure the data is saved
                    }
                }
                StartCoroutine(Switching());
            }
            else
            {
                Debug.LogWarning("Player entered door zone!");
            }
        }
    }

    IEnumerator Switching() 
    {
        yield return new WaitForSeconds(1f);
        Destroy(player);
        SceneManager.UnloadSceneAsync("UI");
        SceneManager.LoadScene("LevelSelection");
    }
}