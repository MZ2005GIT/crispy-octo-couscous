using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D attackHitbox;
    private float attackCooldown = 1.5f;
    private float lastAttackTime = -1f;
    private GameObject player;
    private int facing = 1;
    private bool playerSpotted = false;
    private float moveSpeed = 1f;
    private Vector3 playerPosition;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        
        Transform attackTransform = transform.Find("attack");
        if (attackTransform != null)
        {
            attackHitbox = attackTransform.GetComponent<PolygonCollider2D>();
            if (attackHitbox != null)
            {
                attackHitbox.enabled = false;
            }
        }
    }

    private void Update()
    {
        if (animator.GetBool("isDead")) { return; }

        playerPosition = player.transform.position - transform.position;
        bool isInAttackRange = playerPosition.magnitude < 1f && Mathf.Abs(playerPosition.x) < 1f;
        
        // Face the player
        if (playerPosition.x > 0 && transform.localScale.x < 0)
        {
            facing = 1;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (playerPosition.x < 0 && transform.localScale.x > 0)
        {
            facing = -1;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // Check if player is within detection range
        if (playerPosition.magnitude < 5f && !playerSpotted)
        {
            playerSpotted = true;
            TransitionToWalkAfterReact();
        }
        if (playerSpotted && isInAttackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            rb.velocity = Vector2.zero;
            StartCoroutine(ManageAttackHitbox());
        }
        else if (playerSpotted && !isInAttackRange)
        {
            Vector2 direction = playerPosition.normalized;
            rb.velocity = direction * moveSpeed;
            animator.SetBool("walking", true);
        }
    }

    private void TransitionToWalkAfterReact()
    {
        if (animator != null)
        {
            animator.SetBool("playerSpotted", true);
            animator.SetTrigger("react");
        }
        if (!animator.GetBool("isDead"))
        {
            if (animator != null)
            {
                animator.SetBool("walking", true);
            }
        }
    }

    private IEnumerator ManageAttackHitbox()
    {
        if (animator != null)
        {
            animator.SetTrigger("attack");
        }
        yield return new WaitForSeconds(0.5f);
        if (attackHitbox != null) attackHitbox.enabled = true;
        yield return new WaitForSeconds(0.517f);
        if (attackHitbox != null) attackHitbox.enabled = false;

        lastAttackTime = Time.time;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("attackboxes") &&
            other.transform.parent != null &&
            other.transform.parent.CompareTag("Player"))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(int damage)
    {
        health = Mathf.Max(0, health - damage);
        Debug.Log($"Enemy took {damage} damage. Health remaining: {health}");
        StartCoroutine(PlayHitAnimation());
    }

    private IEnumerator PlayHitAnimation()
    {
        if (animator == null) yield break;
        animator.SetTrigger("hit");

        yield return new WaitForSeconds(0.517f);

        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        if (animator == null) yield break;
        animator.SetBool("isDead", true);

        yield return new WaitForSeconds(1.017f);

        Destroy(gameObject);
    }
}