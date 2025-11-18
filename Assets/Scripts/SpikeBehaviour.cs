using System.Collections;
using UnityEngine;

public class SpikeBehaviour : MonoBehaviour
{
    private Collider2D collider;
    private Animator animator;
    private bool active = false;

    IEnumerator StartAnimationSequence()
    {
        while (animator != null)
        {
            active = false;
            collider.enabled = false;
            animator.SetBool("spikesUp", false);

            yield return new WaitForSeconds(3.0f);

            active = true;
            
            animator.SetBool("spikesUp", true);

            yield return new WaitForSeconds(0.5f);
            collider.enabled = true;
            yield return new WaitForSeconds(0.517f);
        }
    }

    // Use OnTriggerEnter2D for 2D colliders
    void OnTriggerEnter2D(Collider2D other)
    {
        // Only deal damage if the spikes are currently up AND the object is the Player
        if (active && other.CompareTag("Player"))
        {
            // Find the PlayerHealth script on the collided object
            var playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                // Apply instant damage
                playerHealth.TakeDamage(25);
                Debug.Log("Player hit spikes for 25 damage!");
            }
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        StartCoroutine(StartAnimationSequence());
    }
}