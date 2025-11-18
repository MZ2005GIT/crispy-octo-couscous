using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    [Header("Requirements")]
    public int keysRequired = 5; // Must have EXACTLY 5 keys from this level

    [Header("Rewards (Randomized by GameManager)")]
    [HideInInspector] public int rewardCoins; // Assigned randomly by GameManager

    private Animator animator;
    private bool isOpened = false;
    private bool playerInZone = false;
    private PlayerInventoryManager currentPlayerInventory;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator missing on Chest!", this);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            currentPlayerInventory = PlayerInventoryManager.Instance; // FIXED: Use singleton
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            currentPlayerInventory = null;
        }
    }

    void Update()
    {
        if (playerInZone && !isOpened && Input.GetKeyDown(KeyCode.B) && currentPlayerInventory != null)
        {
            if (GameManager.Instance.CanOpenChest())
            {
                AttemptToOpen();
            }
            else
            {
                Debug.Log("Only one chest can be opened per level!");
            }
        }
    }

    private void AttemptToOpen()
    {
        // REQUIRE EXACTLY 5 KEYS (all from this level)
        if (currentPlayerInventory.currentKeys >= keysRequired)
        {
            isOpened = true;

            // TRIGGER ANIMATION
            if (animator != null) animator.SetTrigger("open");

            // CONSUME 1 KEY, GIVE RANDOM COINS
            currentPlayerInventory.UseKeys(1);
            currentPlayerInventory.AddCoins(rewardCoins);

            // DISABLE COLLIDER
            GetComponent<Collider2D>().enabled = false;

            // MARK AS OPENED
            GameManager.Instance.MarkChestAsOpened();

            Debug.Log($"Chest opened! +{rewardCoins} coins (used 1 key, {currentPlayerInventory.currentKeys} keys remain)");
        }
        else
        {
            Debug.Log($"Need {keysRequired} keys to open chest! (Have: {currentPlayerInventory.currentKeys})");
        }
    }
}