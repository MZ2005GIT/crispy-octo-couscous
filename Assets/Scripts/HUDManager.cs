using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HUDManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text coinsText;
    public TMP_Text keysText;
    public Slider healthBar;

    [Header("Health Bar Animation")]
    public float healthSmoothSpeed = 5f;
    private float displayedHealth;

    public PlayerHealth playerHealth;
    public PlayerInventoryManager playerInventory;

    public GameObject player;

    void Start()
    {
        // Initialize health bar
        displayedHealth = playerHealth.GetCurrentHealth();
        //healthBar.maxValue = playerHealth.GetMaxHealth();
        //healthBar.value = displayedHealth;
    }

    public void SetMaxHealth(float max)
    {
        healthBar.value = max;
        healthBar.maxValue = max;
    }
    public void SetHealth(float current)
    {
        healthBar.value = current;
    }

    void Update()
    {
        if (coinsText != null)
        {
            coinsText.text = "Coins: " + GameManager.Instance.totalActualCoins;
        }
        
        if (keysText != null)
        {
            keysText.text = "Keys: " + playerInventory.currentKeys;
        }
        
        // Animate health bar
        if (playerHealth != null && healthBar != null)
        {
            displayedHealth = Mathf.Lerp(displayedHealth, playerHealth.GetCurrentHealth(), Time.deltaTime * healthSmoothSpeed);
            healthBar.value = displayedHealth;
        }
        
    }
}