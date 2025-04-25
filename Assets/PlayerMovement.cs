using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public CharacterController2D controller; 
    public Animator animator;
    public Rigidbody2D rb;
    public BoxCollider2D boxCollider; // Reference to the BoxCollider2D

    public float runSpeed = 40f;
    public float tiltSpeed = 200f; // Speed of tilting while sliding
    float horizontalMove = 0f;
    bool jump = false;
    bool slide = false;
    bool isInWater = false;
    bool onLadder = false;

    // Store original and sliding collider values
    Vector2 originalColliderSize;
    Vector2 originalColliderOffset;
    public Vector2 slidingColliderSize = new Vector2(1.0f, 0.5f); 
    public Vector2 slidingColliderOffset = new Vector2(0f, -0.5f); 

    void Awake() {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (boxCollider == null) boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null) {
            originalColliderSize = boxCollider.size;
            originalColliderOffset = boxCollider.offset;
        }
    }

    void Update() {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        float currentSpeed = animator.GetFloat("Speed");
        bool isGrounded = controller.IsGrounded;

        if (isGrounded) {
            animator.SetBool("isJumping", false);
        }

        // Allow jumping off the ladder with reduced jump height
        if (onLadder && Input.GetButtonDown("Jump")) {
            onLadder = false;
            rb.gravityScale = 3f;
            float ladderJumpForce = 10f; // Reduced jump force for ladder
            rb.velocity = new Vector2(rb.velocity.x, ladderJumpForce);
            animator.SetBool("isJumping", true);
        }
        else if (Input.GetButtonDown("Jump") && isGrounded) {
            jump = true;
            Debug.Log("Jumping");
            animator.SetBool("isJumping", true);
        }

        // Ladder movement
        if(onLadder) {
            rb.gravityScale = 3f;
            if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
                rb.velocity = new Vector2(rb.velocity.x, 5f); // Adjust speed as needed
            }
            else if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
                rb.velocity = new Vector2(rb.velocity.x, -5f); // Adjust speed as needed
            }
            else {
                rb.velocity = new Vector2(rb.velocity.x, 0f); // Stop moving vertically
                rb.gravityScale = 0f ;
            }
        }

        // Start sliding on key down
        if ((Input.GetKeyDown(KeyCode.LeftShift) && (currentSpeed>0.01f )) || (isInWater)) {
            slide = true;
            animator.SetBool("isSliding", true);
            animator.SetBool("isJumping", false); // Cancel jump when sliding starts
            // Change collider for sliding
            if (boxCollider != null) {
                boxCollider.size = slidingColliderSize;
                boxCollider.offset = slidingColliderOffset;
            }
        }
        // Stop sliding on key up or if speed drops
        if (Input.GetKeyUp(KeyCode.LeftShift) || currentSpeed < 0.01f) {
            slide = false;
            animator.SetBool("isSliding", false);
            if (rb != null) rb.rotation = 0f;
            // Revert collider to original
            if (boxCollider != null) {
                boxCollider.size = originalColliderSize;
                boxCollider.offset = originalColliderOffset;
            }
        }

        // Allow tilting while sliding and in water or in air (not grounded)
        if (slide && (isInWater || !isGrounded)) {
            float tiltAmount = 0f;
            float currentTiltSpeed = isInWater ? tiltSpeed * 0.5f : tiltSpeed; // Tilt slower in water
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
                tiltAmount = -currentTiltSpeed * Time.deltaTime; // Tilt down
            } else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
                tiltAmount = currentTiltSpeed * Time.deltaTime; // Tilt up
            }
            rb.MoveRotation(rb.rotation + tiltAmount);
        }
        // If sliding but on ground and not in water, keep rotation flat
        if (slide && isGrounded && !isInWater && rb != null &&
            !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.W) &&
            !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.S)) {
            rb.rotation = 0f;
        }
    }

    void FixedUpdate() {
        controller.Move(horizontalMove * Time.fixedDeltaTime, slide, jump);

        bool isGrounded = controller.IsGrounded;
    
        // Apply force in the direction of tilt when sliding and in water or in air
        if (slide && rb != null && (isInWater || !isGrounded)) {
            // Use tilt direction for air/water sliding
            float tiltRadians = rb.rotation * Mathf.Deg2Rad;
            Vector2 tiltDirection = new Vector2(Mathf.Cos(tiltRadians), Mathf.Sin(tiltRadians));
            float forceMagnitude = isInWater ? 40f : 25f; // Faster sliding in water
            rb.AddForce(tiltDirection * forceMagnitude, ForceMode2D.Force);
        }
    
        // BOOST SPEED WHEN SLIDING ON GROUND (add this block)
        if (slide && isGrounded && !isInWater && rb != null) {
            // Use facing direction for ground sliding
            float slideBoost = 1.3f; // Adjust as needed
            float facing = transform.localScale.x > 0 ? 1f : -1f; // Assumes right is positive scale
            rb.velocity = new Vector2(facing * Mathf.Abs(rb.velocity.x) * slideBoost, rb.velocity.y);
        }
        // BOOST SPEED WHEN SLIDING IN WATER (add this block)
        if (slide && isInWater && rb != null) {
            float waterSlideBoost = 1.3f; // Faster boost in water
            float facing = transform.localScale.x > 0 ? 1f : -1f;
            rb.velocity = new Vector2(facing * Mathf.Abs(rb.velocity.x) * waterSlideBoost, rb.velocity.y);
        }

        jump = false;
        // Do not reset slide here; let Update handle it
    }

    // Add these methods at the end of your PlayerMovement class
    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Trigger entered: " + other.gameObject.name);
        if (other.CompareTag("Water")) {
            isInWater = true;
        }
        if (other.CompareTag("Ladder")) {
            onLadder = true;
        }
        if (other.CompareTag("Fall Zone")) {
            // Reset player position to (0, 0, 0) or your desired respawn point
            transform.position = Vector3.zero;
            rb.velocity = Vector2.zero; // Optional: reset velocity
            Debug.Log("Player fell into Fall Zone. Position reset.");
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Water")) {
            isInWater = false;
        }
        if (other.CompareTag("Ladder")) {
            onLadder = false;
        }
    }
}
