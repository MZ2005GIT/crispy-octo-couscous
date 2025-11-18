using System.Collections;
using UnityEngine;

public class FlamethrowerBehaviour : MonoBehaviour
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
            animator.SetBool("fire out", false);

            yield return new WaitForSeconds(5.0f);

            active = true;
            
            animator.SetBool("fire out", true);

            yield return new WaitForSeconds(0.350f);
            collider.enabled = true;
            yield return new WaitForSeconds(0.550f);

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (active && other.CompareTag("Player"))
        {
            var playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(25);
                Debug.Log("Player hit flamethrower for 25 damage!");
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