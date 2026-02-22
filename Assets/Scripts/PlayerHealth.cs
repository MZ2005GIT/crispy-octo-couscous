using System.Collections;
using UnityEngine;

using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float damageCooldown = 0.5f;  
    private float currentHealth;
    private float lastDamageTime = -1f;
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
            return;
        }
        currentHealth = Mathf.Max(0, currentHealth - damage);
        lastDamageTime = Time.time;
        animator.SetTrigger("hit");
        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        animator.SetBool("isDead", true);
        yield return new WaitForSeconds(1.517f);

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