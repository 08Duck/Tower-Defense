using UnityEngine;
using System.Collections;

public class DASHPLAyaA : MonoBehaviour
{
    public float dashDistance = 5f;
    public float dashCooldown = 1f;
    public float dashDuration = 0.1f; // How long the dash takes (makes it less instant)

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isDashing = false;
    private float lastDashTime = -Mathf.Infinity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Movement input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        // Dash input
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDashTime + dashCooldown && moveInput != Vector2.zero && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;

        Vector2 start = rb.position;
        Vector2 target = start + moveInput * dashDistance;

        // Raycast to avoid dashing through obstacles
        RaycastHit2D hit = Physics2D.Raycast(start, moveInput, dashDistance, LayerMask.GetMask("Wall", "Enemy"));
        if (hit.collider != null)
        {
            target = hit.point;
        }

        float elapsed = 0f;
        while (elapsed < dashDuration)
        {
            rb.MovePosition(Vector2.Lerp(start, target, elapsed / dashDuration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.MovePosition(target); // Snap to final position
        isDashing = false;
    }
}
