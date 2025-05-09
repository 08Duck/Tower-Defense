using UnityEngine;

public class EnemyAttack2D : MonoBehaviour
{
    public Transform player; // Drag the Player here
    public Transform sword; // Drag the Sword here
    public float attackRange = 1.5f; // How close the player must be
    public float attackCooldown = 1f; // Delay between attacks
    public float slashDistance = 1f; // How far the sword moves forward

    private float lastAttackTime;
    private bool facingRight = true;
    private Vector3 rightSwordPos;
    private Vector3 leftSwordPos;

    void Start()
    {
        rightSwordPos = sword.localPosition; // Save right position
        leftSwordPos = new Vector3(-rightSwordPos.x, rightSwordPos.y, rightSwordPos.z); // Create left position
    }

    void Update()
    {
        FlipTowardsPlayer();

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRange && Time.time - lastAttackTime >= attackCooldown)
        {
            Slash();
            lastAttackTime = Time.time;
        }
    }

    void FlipTowardsPlayer()
    {
        if (player.position.x > transform.position.x && !facingRight)
        {
            Flip();
        }
        else if (player.position.x < transform.position.x && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        // Flip the enemy
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        // Flip the sword position
        sword.localPosition = facingRight ? rightSwordPos : leftSwordPos;
    }

    void Slash()
    {
        Debug.Log("Enemy slashes!");

        // Move sword forward when attacking
        Vector3 offset = new Vector3(facingRight ? slashDistance : -slashDistance, 0f, 0f);
        sword.localPosition += offset;

        Invoke("ResetSword", 0.1f);
    }

    void ResetSword()
    {
        // Reset sword back to normal position
        sword.localPosition = facingRight ? rightSwordPos : leftSwordPos;
    }
}
