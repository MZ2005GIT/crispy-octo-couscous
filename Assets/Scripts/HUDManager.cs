using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text coinsText;
    public TMP_Text keysText;
    public Slider healthBar;

    [Header("Health Bar Animation")]
    public float healthSmoothSpeed = 5f;
    private float displayedHealth;

    private PlayerHealth playerHealth;
    private PlayerInventoryManager playerInventory;

    [Header("Player Tag")]
    [SerializeField] private string playerTag = "Player"; // Assign in Inspector or hardcode

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        StartCoroutine(InitializeComponents());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reinitialize components when a new scene is loaded
        StartCoroutine(InitializeComponents());
        Debug.Log($"Scene loaded: {scene.name}, Active Scene: {SceneManager.GetActiveScene().name}");
    }

    private IEnumerator InitializeComponents()
    {
        // Keep trying to find the player and its components
        int attempts = 0;
        const int maxAttempts = 50; // Prevent infinite loop
        while ((playerHealth == null || playerInventory == null) && attempts < maxAttempts)
        {
            // Find the player GameObject by tag
            GameObject player = GameObject.FindWithTag(playerTag);

            if (player != null)
            {
                // Get components from the player GameObject
                playerHealth = player.GetComponent<PlayerHealth>();
                playerInventory = player.GetComponent<PlayerInventoryManager>();

                if (playerHealth == null || playerInventory == null)
                {
                    Debug.LogWarning($"Player found with tag '{playerTag}', but missing PlayerHealth or PlayerInventoryManager in scene {SceneManager.GetActiveScene().name}.");
                }
            }
            else
            {
                Debug.LogWarning($"Player with tag '{playerTag}' not found in scene {SceneManager.GetActiveScene().name}. Retrying...");
            }

            if (playerHealth == null || playerInventory == null)
            {
                yield return new WaitForSeconds(0.1f); // Wait before retrying
                attempts++;
            }
        }

        if (playerHealth == null || playerInventory == null)
        {
            Debug.LogError($"Failed to find PlayerHealth or PlayerInventoryManager after {maxAttempts} attempts!");
            yield break;
        }

        // Initialize health bar
        displayedHealth = playerHealth.GetCurrentHealth();
        healthBar.maxValue = playerHealth.GetMaxHealth();
        healthBar.value = displayedHealth;

        Debug.Log("HUDManager initialized successfully.");
    }

    void Update()
    {
        // Update coins (from GameManager)
        if (coinsText != null && GameManager.Instance != null)
        {
            coinsText.text = "Coins: " + GameManager.Instance.totalActualCoins;
        }
        else
        {
            Debug.LogWarning("CoinsText or GameManager.Instance is null.");
        }

        // Update keys (from PlayerInventoryManager)
        if (keysText != null && playerInventory != null)
        {
            keysText.text = "Keys: " + playerInventory.currentKeys;
        }
        else
        {
            Debug.LogWarning("KeysText or PlayerInventory is null.");
        }

        // Animate health bar
        if (playerHealth != null && healthBar != null)
        {
            displayedHealth = Mathf.Lerp(displayedHealth, playerHealth.GetCurrentHealth(), Time.deltaTime * healthSmoothSpeed);
            healthBar.value = displayedHealth;
        }
        else
        {
            Debug.LogWarning("PlayerHealth or HealthBar is null.");
        }
    }
}