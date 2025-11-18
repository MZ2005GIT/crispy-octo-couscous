using UnityEngine;

// This script MUST be attached to the MiniChest GameObject
// The MiniChest GameObject must also have a Collider set to 'Is Trigger' and an Animator.
public class MiniChest : MonoBehaviour
{
    public int rewardKeys = 1;
    private Animator animator;
    private bool isOpened = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component missing on the MiniChest GameObject.", this);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isOpened || !other.CompareTag("Player")) return;
        AttemptToOpen();
    }

    private void AttemptToOpen()
    {
        if (isOpened) return; // Double-check to prevent re-opening
        isOpened = true; // Set flag to prevent re-opening

        PlayerInventoryManager playerInventory = PlayerInventoryManager.Instance; // Use singleton
        if (playerInventory != null)
        {
            // Apply reward
            playerInventory.AddKeys(rewardKeys);
            // Trigger animation if animator exists
            if (animator != null)
            {
                animator.SetTrigger("open"); // Assumes Animator has a trigger parameter named "Open"
            }
        }
        else
        {
            Debug.LogWarning("PlayerInventoryManager instance not found.", this);
        }
    }
}