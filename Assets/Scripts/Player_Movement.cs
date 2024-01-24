using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float Speed = 15f;
    public float JumpForce = 10f;
    public float MaxJumpTime = 0.5f;
    public float DashSpeed = 30f;
    public float DashDuration = 0.5f;
    private bool isFacingRight = true;

    private Rigidbody2D rb;
    private float currentJumpTime = 0f;
    private float dashTimeCounter = 0f;
    private bool isJumping = false;
    private bool isDashing = false;

    [SerializeField] private Transform groundCheck;
    private Transform playerTransform;
    public float GroundCheckRadius = 0.2f;
    public LayerMask GroundLayer;

    [SerializeField] private float coyoteTime = .1f;
    private float coyoteTimeCounter = 0f;

    [SerializeField] private Transform wallCheckL;
    [SerializeField] private Transform wallCheckR;
    [SerializeField] private float wallSlidingSpeed = 1.5f; 
    [SerializeField] private float wallJumpForce = 10f;
    [SerializeField] private float wallJumpTime = 0.2f;
    public LayerMask WallLayer;
    private float wallJumpTimeCounter;
    public float WallCheckRadius = 0.2f;
    private bool isWallSliding;
    private bool isWallJumping;

    public LayerMask VictoryLayer;
    public LayerMask DefeatLayer;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = transform;
    }

    private void Update() {

        if (GameManager._instance.isPlaying()) {

            if (IsGrounded()) {
                coyoteTimeCounter = coyoteTime;
            } else {
                coyoteTimeCounter -= Time.deltaTime;
            }

            if (coyoteTimeCounter < 0 || !IsGrounded()) {
                if (!isWallSliding) {
                    ApplyGravity();
                }
            }

            Movement();
            WallSlide();
            WallJump();
        }

        if(IsVictory()) {
            GameManager._instance.changeGameState(GameState.Victory);
        }
        if(IsDefeat()) {
            GameManager._instance.changeGameState(GameState.GameOver);
        }
        

    }

    private void Movement() {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        Dash();
        rb.velocity = new Vector2(horizontalInput * Speed, rb.velocity.y);

        //Vector2 move = new Vector2(horizontalInput, 0);
        //playerTransform.Translate(move * Speed * Time.deltaTime);

        FlipPlayer(horizontalInput);

        Jump();
    }

    private void Dash() {
        if (Input.GetMouseButtonDown(0) && !isDashing) {
            isDashing = true;
            dashTimeCounter = DashDuration;
        }

        if (isDashing) {
            playerTransform.Translate(Vector2.right * (playerTransform.localScale.x * DashSpeed) * Time.deltaTime);
            dashTimeCounter -= Time.deltaTime;

            if (dashTimeCounter <= 0) {
                isDashing = false;
            }
        }
    }

    private void Jump() {
        if (Input.GetKeyDown(KeyCode.Space) && (IsGrounded() || coyoteTimeCounter > 0) && !IsWalled()) {
            isJumping = true;
            currentJumpTime = MaxJumpTime;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping) {
            if (currentJumpTime > 0) {
                playerTransform.Translate(Vector2.up * JumpForce * Time.deltaTime);
                currentJumpTime -= Time.deltaTime;
            } else {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            isJumping = false;
        }
    }

    private void FlipPlayer(float horizontalInput) {
        if (horizontalInput < 0) {
            isFacingRight = true;
            playerTransform.localScale = new Vector3(-1, 1, 1);
        } else if (horizontalInput > 0) {
            isFacingRight = false;
            playerTransform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void ApplyGravity() {
        float gravity = Physics2D.gravity.y;
        playerTransform.Translate(Vector2.up * gravity * Time.deltaTime);
    }

    private bool IsWalled() {
        return Physics2D.OverlapCircle(wallCheckL.position, WallCheckRadius, WallLayer) ||
               Physics2D.OverlapCircle(wallCheckR.position, WallCheckRadius, WallLayer);
    }

    private bool IsGrounded() {
        return Physics2D.OverlapCircle(groundCheck.position, GroundCheckRadius, GroundLayer);
    }

    private bool IsVictory() {
        return Physics2D.OverlapCircle(groundCheck.position, GroundCheckRadius, VictoryLayer);
    }
    private bool IsDefeat() {
        return Physics2D.OverlapCircle(groundCheck.position, GroundCheckRadius, DefeatLayer);
    }

    private void WallSlide() {
        if (IsWalled() && !IsGrounded() && !isWallJumping) {
            isWallSliding = true;
            playerTransform.position += Vector3.down * wallSlidingSpeed * Time.deltaTime;
        } else {
            isWallSliding = false;
        }
    }

    private void WallJump() {
        if (Input.GetKeyDown(KeyCode.Space) && isWallSliding) {
            isWallJumping = true;
            wallJumpTimeCounter = wallJumpTime;
        }

        if (isWallJumping) {
            if (Input.GetKey(KeyCode.Space) && wallJumpTimeCounter > 0) {
                playerTransform.position += Vector3.up * wallJumpForce * Time.deltaTime;
                if(isFacingRight) {
                    playerTransform.position += Vector3.right * 20 * Time.deltaTime;
                } else {
                    playerTransform.position += Vector3.left * 20 * Time.deltaTime;
                }
                wallJumpTimeCounter -= Time.deltaTime;
            } else {
                isWallJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            isWallJumping = false;
        }
    }
}

