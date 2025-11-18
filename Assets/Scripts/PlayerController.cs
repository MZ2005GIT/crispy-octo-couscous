using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int speed = 6;
    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D attackHitbox;
    private int facing = 1;
    private float attackCooldown = 1.5f;
    private float lastAttackTime = -1f;
    private PlayerHealth healthSystem;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        healthSystem = GetComponent<PlayerHealth>();

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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !animator.GetBool("attack") && Time.time >= lastAttackTime + attackCooldown)
        {
            rb.velocity = Vector2.zero;
            animator.SetFloat("horizontal", 0);
            animator.SetFloat("vertical", 0);
            lastAttackTime = Time.time;
            StartCoroutine(StartAnimationAttack());
        }
    }

    private void FixedUpdate()
    {
        if (!animator.GetBool("attack"))
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            animator.SetFloat("horizontal", Mathf.Abs(horizontal));
            animator.SetFloat("vertical", Mathf.Abs(vertical));

            if ((horizontal > 0 && transform.localScale.x < 0) || (horizontal < 0 && transform.localScale.x > 0))
            {
                facing *= -1;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
            rb.velocity = new Vector2(horizontal, vertical).normalized * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("attackboxes") &&
            other.transform.CompareTag("Enemy"))
        {
            healthSystem.TakeDamage(10);
        }
    }

    private IEnumerator StartAnimationAttack()
    {
        animator.SetBool("attack", true);
        yield return new WaitForSeconds(0.5f);
        if (attackHitbox != null)
        {
            attackHitbox.enabled = true;
        }

        yield return new WaitForSeconds(0.517f);

        animator.SetBool("attack", false);
        if (attackHitbox != null)
        {
            attackHitbox.enabled = false;
        }
    }
}