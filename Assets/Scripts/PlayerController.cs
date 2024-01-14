using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashSpeed = 15f;  
    [SerializeField] private float dashDuration = 1f;  
    [SerializeField] private float jumpSpeed = 7f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;
    private float dashTimeLeft = 0f;  

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {

        if(GameManager._instance.isPlaying()) {

            Movement();

            if (Input.GetButtonDown("Jump") && isGrounded) {
                Jump();
            }
            if (Input.GetKeyDown("k")) {
                Dash();
            }
        }
        
    }

    private void FixedUpdate() {

        if (GameManager._instance.isPlaying()) {
            CheckGrounded();
            HandleDashCooldown();
        }
    }

    private void Movement() {
        float moveX = Input.GetAxisRaw("Horizontal");
        float targetVelocity = moveX * moveSpeed;

        if (Input.GetKey("k") && dashTimeLeft > 0) {
            targetVelocity = moveX * dashSpeed;
        }

        float smoothTime = isGrounded ? 0.01f : 0.005f;

        Vector2 velocity = rb.velocity;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocity, ref velocity.x, smoothTime);

        rb.velocity = velocity;
    }

    private void Jump() {
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }

    private void Dash() {
        if (dashTimeLeft <= 0) {
            dashTimeLeft = dashDuration;
            
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }
    }

    private void CheckGrounded() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void HandleDashCooldown() {
        if (dashTimeLeft > 0) {
            dashTimeLeft -= Time.fixedDeltaTime;
        }
    }
}
