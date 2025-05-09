using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 6f;
    [SerializeField] private float jumpPower = 16f;
    private float horizontalInput;
    private bool isFacingRight = true;

    [Header("Wall Mechanics")]
    [SerializeField] private float wallSlideSpeed = 1.5f;
    [SerializeField] private float wallJumpTime = 0.2f;
    [SerializeField] private float wallJumpDuration = 0.2f;
    [SerializeField] private Vector2 wallJumpForce = new Vector2(8f, 16f);
    private bool isWallSliding;
    private bool isWallJumping;
    private float wallJumpTimer;
    private float wallJumpDirection;

    [Header("Double Jump")]
    [SerializeField] private int maxJumps = 2;
    private int remainingJumps;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    public Text SpeedBoostTimer;
    private int MoveSpedPots = 0;
    private int MoveSpedMaxPots = 1;
    public Text MoveSPEDTEXT;
    bool isBoosted = false;
    [SerializeField] private float speedIncrement = 1f;
    [SerializeField] private int boostIterations = 10;
    [SerializeField] private int boostDuration = 3;
    [SerializeField] private float boostIncreaseDelay = 0.025f;
    [SerializeField] private float boostDecreaseDelay = 0.05f;

    private void UpdateSpeedBoostTimer()
    {
        for (int i = boostDuration; i>0; i--)
        { SpeedBoostTimer.text = "" + i + "s"; }
    }

    private void UpdateMOVESPEDPOTS()
    {
        MoveSPEDTEXT.text = "" + MoveSpedPots;
    }
    private void Start()
    {
        remainingJumps = maxJumps;  // Ensure jumps are reset at start
        UpdateMOVESPEDPOTS();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("MovementSpeedPotion") && MoveSpedPots != MoveSpedMaxPots)
        {
            MoveSpedPots++;
            Debug.Log("Pickup movement speed potion");
            UpdateMOVESPEDPOTS();
            Debug.Log(other.gameObject);
            Destroy(other.gameObject); // Destroy the potion you collided with
        }
    }
    private void Update()
    {
        HandleInput();
        HandleJump();
        HandleWallSlide();
        HandleWallJump();
        if (!isWallJumping)
        {
            FlipCharacter();
        }
        if (Input.GetKeyDown("f") && MoveSpedPots >= 1 && !isBoosted)
        {

            isBoosted = true;
            Debug.Log("Speeding up");
            MoveSpedPots--;
            StartCoroutine(DoSpeedBoost());
            UpdateMOVESPEDPOTS();
            
        }
    }

    IEnumerator DoSpeedBoost()
    {
        float oldSpeed = speed;
        for (int i = 0; i < boostIterations; i++)
        {
            yield return new WaitForSeconds(boostIncreaseDelay);
            Debug.Log($"Speeding up {speed}");
            speed += speedIncrement;
        }

        yield return new WaitForSeconds(boostDuration);

        for (int i = 0; i < boostIterations; i++)
        {
            yield return new WaitForSeconds(boostDecreaseDelay);
            Debug.Log($"Speeding down {speed}");
            speed -= speedIncrement;
        }
        isBoosted = false;
    }

    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        }
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    private void HandleJump()
    {
        if (IsGrounded())
        {
            remainingJumps = maxJumps; // Reset jump count when grounded
        }

        if (Input.GetButtonDown("Jump") && remainingJumps > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            remainingJumps--;  // Use a jump
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.375f, groundLayer);
    }

    private bool IsAgainstWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void HandleWallSlide()
    {
        isWallSliding = IsAgainstWall() && !IsGrounded() && horizontalInput != 0f;
        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
    }

    private void HandleWallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpTimer = wallJumpTime;
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpTimer -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpTimer > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallJumpForce.x, wallJumpForce.y);
            wallJumpTimer = 0f;
            remainingJumps = maxJumps - 1;  // Reset jumps but use one
            FlipCharacterOnJump();
            Invoke(nameof(StopWallJumping), wallJumpDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void FlipCharacter()
    {
        if ((isFacingRight && horizontalInput < 0f) || (!isFacingRight && horizontalInput > 0f))
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            Transform rotationSquare = transform.Find("RotationSquare");
            if (rotationSquare != null)
            {
                rotationSquare.localScale = new Vector3(-rotationSquare.localScale.x, rotationSquare.localScale.y, rotationSquare.localScale.z);
            }
        }
    }

    private void FlipCharacterOnJump()
    {
        if (transform.localScale.x != wallJumpDirection)
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            Transform rotationSquare = transform.Find("RotationSquare");
            if (rotationSquare != null)
            {
                rotationSquare.localScale = new Vector3(-rotationSquare.localScale.x, rotationSquare.localScale.y, rotationSquare.localScale.z);
            }
        }
    }

}


