using System.Collections;
using UnityEngine;

public class PatrollingEnemy : MonoBehaviour
{
    [Header("Stats")]
    public int health = 100;
    public float moveSpeed = 2f;
    public float attackCooldown = 1.2f;
    public float attackRange = 2f; // NEW: Adjustable range

    [Header("References")]
    public GameObject player;

    // Private components
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D attackHitbox;

    // Patrol state
    private float direction = 1f; // 1 = right, -1 = left
    private bool isMoving = true;

    // Attack state
    private float lastAttackTime = -1f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");

        // Start moving RIGHT
        rb.velocity = new Vector2(direction * moveSpeed, 0f);
        animator.SetBool("walking", true);

        SetupAttackHitbox();

        Debug.Log($"START: Pos={transform.position.x:F1}, Direction={direction}");
    }

    private void SetupAttackHitbox()
    {
        Transform attackTransform = transform.Find("attack");
        if (attackTransform != null)
        {
            attackHitbox = attackTransform.GetComponent<Collider2D>();
            if (attackHitbox != null)
            {
                attackHitbox.enabled = false;
            }
        }
    }

    private void Update()
    {
        if (player == null || animator.GetBool("isDead")) return;

        Vector3 playerPos = player.transform.position - transform.position;
        bool inAttackRange = playerPos.magnitude < attackRange && Mathf.Abs(playerPos.x) < attackRange;

        if (inAttackRange && CanAttack())
        {
            // ✅ STOP PATROL + TURN TO PLAYER + ATTACK
            AttackPlayer(playerPos.x);
        }
        else if (!IsInAnimation("attack") && !IsInAnimation("hit") && !animator.GetBool("isDead"))
        {
            // ✅ RESUME PATROL when player out of range
            PatrolMovement();
        }
    }

    private void PatrolMovement()
    {
        if (!isMoving) return;

        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
        animator.SetBool("walking", true);
        TurnToFaceDirection(direction);

        Debug.Log($"PATROL: Pos={transform.position.x:F1}, Direction={direction}");
    }

    // ✅ FIXED: Side collision detection
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (Mathf.Abs(contact.normal.y) < 0.5f) // Side hit
            {
                direction *= -1f;
                rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
                Debug.Log($"✅ WALL HIT! New Direction={direction}");
                return;
            }
        }
    }

    // ✅ NEW: Attack with player-facing turn
    private void AttackPlayer(float playerXRelative)
    {
        isMoving = false;
        rb.velocity = Vector2.zero;
        animator.SetBool("walking", false);

        // TURN TO FACE PLAYER
        float playerDirection = Mathf.Sign(playerXRelative);
        TurnToFaceDirection(playerDirection);

        animator.SetBool("attack", true);
        StartCoroutine(ManageAttackHitbox());

        Debug.Log($"⚔️ ATTACKING! Facing Player at {playerXRelative:F1}");
    }

    private bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldown && !IsInAnimation("hit");
    }

    private void TurnToFaceDirection(float dir)
    {
        if (dir > 0 && transform.localScale.x < 0) // Face RIGHT
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (dir < 0 && transform.localScale.x > 0) // Face LEFT
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private bool IsInAnimation(string animName)
    {
        return animator.GetBool(animName);
    }

    private IEnumerator ManageAttackHitbox()
    {
        yield return new WaitForSeconds(0.5f);
        if (attackHitbox != null) attackHitbox.enabled = true;
        yield return new WaitForSeconds(0.517f);
        if (attackHitbox != null) attackHitbox.enabled = false;
        animator.SetBool("attack", false);
        lastAttackTime = Time.time;
        isMoving = true; // Resume patrol
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
        Debug.Log($"HIT! Health: {health}");
        StartCoroutine(PlayHitAnimation());
    }

    private IEnumerator PlayHitAnimation()
    {
        isMoving = false;
        animator.SetBool("hit", true);
        animator.SetBool("walking", false);
        animator.SetBool("attack", false);
        yield return new WaitForSeconds(0.517f);
        animator.SetBool("hit", false);
        isMoving = true;
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        isMoving = false;
        animator.SetBool("isDead", true);
        animator.SetBool("walking", false);
        animator.SetBool("attack", false);
        yield return new WaitForSeconds(1.1f);
        Debug.Log("ENEMY DIED!");
        PlayerInventoryManager inventory = FindObjectOfType<PlayerInventoryManager>();
        if (inventory != null)
        {
            inventory.AddKeys(1);
        }
        Destroy(gameObject);
    }
}