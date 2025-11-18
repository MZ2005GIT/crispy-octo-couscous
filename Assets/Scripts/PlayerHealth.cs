using System.Collections;
using UnityEngine;

using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float damageCooldown = 0.5f;  // FIXED
    private float currentHealth;
    private float lastDamageTime = -1f;  // FIXED
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (Time.time < lastDamageTime + damageCooldown)
        {
            Debug.Log("Damage blocked due to cooldown");
            return;
        }
        currentHealth = Mathf.Max(0, currentHealth - damage);
        lastDamageTime = Time.time;
        Debug.Log($"Player took {damage} damage. Health remaining: {currentHealth}");
        StartCoroutine(PlayHitAnimation());
        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator PlayHitAnimation()
    {
        animator.SetBool("hit", true);
        yield return new WaitForSeconds(0.517f);
        animator.SetBool("hit", false);
    }

    private IEnumerator Die()
    {
        animator.SetBool("isDead", true);
        yield return new WaitForSeconds(1.517f);
        Debug.Log("Player has died!");
        Destroy(gameObject);
        SceneManager.LoadScene("GameOver");
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetMaxHealth()
    {
        currentHealth = maxHealth;
    }
}