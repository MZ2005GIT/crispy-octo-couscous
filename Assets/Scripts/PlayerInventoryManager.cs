using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    public static PlayerInventoryManager Instance { get; private set; } // Changed to public static property

    public int currentKeys = 0; // Start with 0 keys
    public int currentCoins = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Check if the player has enough keys
    public bool HasKeys(int required)
    {
        return currentKeys >= required;
    }

    // Use keys (called by chests or doors)
    public void UseKeys(int amount)
    {
        if (currentKeys >= amount)
        {
            currentKeys -= amount;
            Debug.Log("Keys used: " + amount + ". Remaining keys: " + currentKeys);
        }
    }

    // Add this method at the end:
    public void AddCoins(int amount)
    {
        currentCoins += amount;
        GameManager.Instance.AddToActualCoins(amount); // Track progress!
        Debug.Log($" +{amount} coins! Total: {currentCoins}");
    }

    // Add keys (called by minichests)
    public void AddKeys(int amount)
    {
        currentKeys += amount;
        Debug.Log("Keys added: " + amount + ". Total keys: " + currentKeys);

    }
}