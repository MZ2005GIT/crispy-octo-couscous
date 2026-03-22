using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private bool isOpened = false;
    private GameObject player;
    private GameManager gameManager;

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

                    // Mark the level as cleared (e.g., for Level 1)
                    string currentLevel = SceneManager.GetActiveScene().name;
                    if (currentLevel == "lvl1") // Adjust the scene name as needed
                    {
                        gameManager.levelUnlock = true;
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
        SceneManager.LoadScene("LevelSelection");
    }
}