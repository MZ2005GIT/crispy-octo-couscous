using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Tracking")]
    public int totalActualCoins = 0;
    [HideInInspector] public int maxPossibleCoins;
    public int totalMaxPossibleCoins = 0;
    private bool chestOpenedThisLevel = false;

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

    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"\n LEVEL {scene.buildIndex + 1} STARTED");

        // RESET PER-LEVEL FLAGS
        chestOpenedThisLevel = false;
       
            PlayerInventoryManager.Instance.currentKeys = 0;
            Debug.Log("Keys reset to 0");

        // RANDOMIZE COINS FOR ALL CHESTS IN THIS LEVEL
        AssignRandomCoinsToChests(scene);

        // TRACK MAX POSSIBLE
        UpdateMaxPossibleCoins(scene);

        Debug.Log($" Level {scene.buildIndex + 1} Max: {GetLevelMaxCoins(scene.buildIndex)} | Total Max: {totalMaxPossibleCoins}");
    }

    // Assign 50-150 random coins to each chest
    private void AssignRandomCoinsToChests(Scene scene)
    {
        TreasureChest[] chests = FindObjectsOfType<TreasureChest>();
        foreach (var chest in chests)
        {
            chest.rewardCoins = Random.Range(50, 151); // 50-150 coins
            Debug.Log($" Chest {chest.name}: {chest.rewardCoins} coins");
        }
    }

    // Calculate max possible for this level
    private void UpdateMaxPossibleCoins(Scene scene)
    {
        int levelMax = GetLevelMaxCoins(scene.buildIndex);
        totalMaxPossibleCoins += levelMax;
        maxPossibleCoins = totalMaxPossibleCoins;
    }

    public int GetLevelMaxCoins(int buildIndex)
    {
        TreasureChest[] chests = FindObjectsOfType<TreasureChest>();
        return chests.Length > 0 ? chests[0].rewardCoins : 0; // Only 1 chest matters per level
    }

    public int GetProgressPercentage()
    {
        return totalActualCoins > 0 && totalMaxPossibleCoins > 0
            ? (int)((float)totalActualCoins / totalMaxPossibleCoins * 100)
            : 0;
    }

    // Called by UI to get stats
    public (int actual, int max, int percent) GetCoinStats()
    {
        return (totalActualCoins, totalMaxPossibleCoins, GetProgressPercentage());
    }

    public bool CanOpenChest() => !chestOpenedThisLevel;
    public void MarkChestAsOpened() => chestOpenedThisLevel = true;

    // Called by PlayerInventoryManager when coins added
    public void AddToActualCoins(int amount)
    {
        totalActualCoins += amount;
        Debug.Log($"TOTAL: {totalActualCoins}/{totalMaxPossibleCoins} ({GetProgressPercentage()}%)");
    }
}