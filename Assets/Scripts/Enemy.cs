using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D attackHitbox;
    private float attackCooldown = 1.2f;
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
        if (animator == null) Debug.LogWarning("Animator component not found on Enemy!");
        if (rb == null) Debug.LogWarning("Rigidbody2D component not found on Enemy!");
        if (player == null)
        {
            Debug.LogWarning("Player GameObject not found in Enemy Start!");
        }
        else
        {
            Debug.Log($"Found Player: {player.name}, Scene: {player.scene.name}, Position: {player.transform.position}");
        }
        Transform attackTransform = transform.Find("attack");
        if (attackTransform != null)
        {
            attackHitbox = attackTransform.GetComponent<PolygonCollider2D>();
            if (attackHitbox != null)
            {
                attackHitbox.enabled = false;
            }
            else
            {
                Debug.LogWarning("Attack hitbox PolygonCollider2D not found!");
            }
        }
        else
        {
            Debug.LogWarning("Attack transform not found!");
        }
        Debug.Log($"{gameObject.name} Position: {transform.position}, Player Position: {player.transform.position}, Distance: {(player.transform.position - transform.position).magnitude}");
    }

    private void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player reference is null!");
            return;
        }
        playerPosition = player.transform.position - transform.position;
        bool isInAttackRange = playerPosition.magnitude < 1f && Mathf.Abs(playerPosition.x) < 1f;
        Debug.Log($"{gameObject.name} State: playerSpotted={playerSpotted}, " +
                  $"isInAttackRange={isInAttackRange}, attack={animator.GetBool("attack")}, " +
                  $"react={animator.GetBool("react")}, hit={animator.GetBool("hit")}, " +
                  $"isDead={animator.GetBool("isDead")}, playerPosition={playerPosition}, " +
                  $"magnitude={playerPosition.magnitude}, velocity={rb.velocity}");

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
            StartCoroutine(TransitionToWalkAfterReact());
        }
        if (playerSpotted && isInAttackRange && Time.time >= lastAttackTime + attackCooldown &&
            !animator.GetBool("react") && !animator.GetBool("hit") && !animator.GetBool("isDead"))
        {
            rb.velocity = Vector2.zero;
            StartCoroutine(ManageAttackHitbox());
        }
        else if (playerSpotted && !isInAttackRange && !animator.GetBool("attack") &&
                 !animator.GetBool("react") && !animator.GetBool("hit") && !animator.GetBool("isDead"))
        {
            Vector2 direction = playerPosition.normalized;
            rb.velocity = direction * moveSpeed;
            animator.SetBool("walking", true);
            Debug.Log($"{gameObject.name} Moving: direction={direction}, velocity={rb.velocity}, moveSpeed={moveSpeed}");
        }
    }

    private IEnumerator TransitionToWalkAfterReact()
    {
        if (animator != null)
        {
            animator.SetBool("playerSpotted", true);
            animator.SetBool("react", true);
            animator.SetBool("walking", false);
            Debug.Log("Player spotted, starting react animation");
        }
        yield return new WaitForSeconds(1f); // Verify this matches react animation length
        if (playerSpotted && !animator.GetBool("attack") && !animator.GetBool("hit") && !animator.GetBool("isDead"))
        {
            if (animator != null)
            {
                animator.SetBool("react", false);
                animator.SetBool("walking", true);
            }
            Debug.Log("React animation finished, transitioning to walk");
        }
        else
        {
            Debug.Log($"Cannot transition to walk: attack={animator.GetBool("attack")}, hit={animator.GetBool("hit")}, isDead={animator.GetBool("isDead")}");
        }
    }

    private IEnumerator ManageAttackHitbox()
    {
        if (animator != null)
        {
            animator.SetBool("walking", false);
            animator.SetBool("react", false);
            animator.SetBool("attack", true);
            Debug.Log("Starting attack animation");
        }
        yield return new WaitForSeconds(0.5f);
        if (attackHitbox != null) attackHitbox.enabled = true;
        yield return new WaitForSeconds(0.517f);
        if (attackHitbox != null) attackHitbox.enabled = false;
        if (animator != null)
        {
            animator.SetBool("attack", false);
        }
        lastAttackTime = Time.time;
        Debug.Log("Attack animation finished");
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
        animator.SetBool("hit", true);
        animator.SetBool("walking", false);
        animator.SetBool("react", false);
        animator.SetBool("attack", false);
        yield return new WaitForSeconds(0.517f);
        animator.SetBool("hit", false);
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        if (animator == null) yield break;
        animator.SetBool("isDead", true);
        animator.SetBool("walking", false);
        animator.SetBool("react", false);
        animator.SetBool("attack", false);
        yield return new WaitForSeconds(1.017f);
        Debug.Log("Enemy has died!");
        Destroy(gameObject);
    }
}