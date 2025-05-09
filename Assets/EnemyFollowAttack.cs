using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowAttack : MonoBehaviour
{
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float moveSpeed = 3f;
    public float attackCooldown = 1.5f;
    public float slashDuration = 0.4f;
    public float jumpForce = 8f;
    public float jumpCooldown = 2f;

    public Transform sword;          // The sword itself
    public Transform swordPivot;     // The pivot point where the sword rotates (near hand/shoulder)
    public Transform swordHolder;    // The holder that will rotate the sword (child of swordPivot)
    public float slashRadius = 0.4f; // The radius the sword will swing around (closer to the enemy)

    private Transform player;
    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private float attackTimer = 0f;
    private float jumpTimer = 0f;
    private bool isSlashing = false;
    private float slashTimer = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        sword.gameObject.SetActive(false); // Hide sword by default
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        attackTimer -= Time.deltaTime;
        jumpTimer -= Time.deltaTime;

        if (distance < detectionRange)
        {
            FollowPlayer();

            if (distance <= attackRange && attackTimer <= 0f)
            {
                StartSlash();
            }

            TryJump();
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        FlipIfNeeded();
        HandleSwordArcSwing();
    }

    void FollowPlayer()
    {
        if (!isSlashing)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
        }
    }

    void StartSlash()
    {
        isSlashing = true;
        slashTimer = slashDuration;
        attackTimer = attackCooldown;

        sword.gameObject.SetActive(true); // Show sword when slashing
    }

    void HandleSwordArcSwing()
    {
        if (!isSlashing) return;

        // Flip sword's Y scale depending on slash direction
        Vector3 swordScale = sword.localScale;
        swordScale.y = isFacingRight ? Mathf.Abs(swordScale.y) : -Mathf.Abs(swordScale.y);
        sword.localScale = swordScale;

        float progress = 1f - (slashTimer / slashDuration);
        float angleDeg = Mathf.Lerp(90f, -90f, progress);
        float rotationAngle = isFacingRight ? angleDeg : 180f - angleDeg;

        swordHolder.position = swordPivot.position;

        float angleRad = rotationAngle * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * slashRadius;

        sword.position = new Vector2(swordHolder.position.x, swordHolder.position.y) + offset;
        swordHolder.rotation = Quaternion.Euler(0f, 0f, rotationAngle);

        slashTimer -= Time.deltaTime;

        if (slashTimer <= 0f)
        {
            isSlashing = false;
            sword.gameObject.SetActive(false);
        }
    }

    void TryJump()
    {
        if (player.position.y > transform.position.y + 1f && jumpTimer <= 0f)
        {
            if (Random.value < 0.3f)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimer = jumpCooldown;
            }
        }
    }

    void FlipIfNeeded()
    {
        bool playerOnRight = player.position.x > transform.position.x;
        if (playerOnRight != isFacingRight)
        {
            isFacingRight = playerOnRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}
