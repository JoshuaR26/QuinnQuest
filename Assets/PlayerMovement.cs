using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public CharacterController2D controller; 
    public Animator animator;
    public Rigidbody2D rb; // Add this line to reference the Rigidbody2D

    public float runSpeed = 40f;
    public float tiltSpeed = 200f; // Speed of tilting while sliding
    float horizontalMove = 0f;
    bool jump = false;
    bool slide = false;
    bool isInWater = false; // <-- Move this here to make it a class-level field

    void Awake() {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        float currentSpeed = animator.GetFloat("Speed");
        bool isGrounded = controller.IsGrounded;

        if (Input.GetButtonDown("Jump")) {
            jump = true;
            animator.SetBool("isJumping", true);
        }

        // Start sliding on key down
        if (Input.GetKeyDown(KeyCode.LeftShift) && currentSpeed > 0.1f) {
            slide = true;
            animator.SetBool("isSliding", true);
            animator.SetBool("isJumping", false); // Cancel jump when sliding starts
        }
        // Stop sliding on key up or if speed drops
        if (Input.GetKeyUp(KeyCode.LeftShift) || currentSpeed < 0.01f) {
            slide = false;
            animator.SetBool("isSliding", false);
            if (rb != null) rb.rotation = 0f;
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
        if (slide && isGrounded && !isInWater && rb != null) {
            rb.rotation = 0f;
        }
    }

    void FixedUpdate() {
        controller.Move(horizontalMove * Time.fixedDeltaTime, slide, jump);

        bool isGrounded = controller.IsGrounded;
        // Apply force in the direction of tilt when sliding and in water or in air
        if (slide && rb != null && (isInWater || !isGrounded)) {
            float tiltRadians = rb.rotation * Mathf.Deg2Rad;
            Vector2 tiltDirection = new Vector2(Mathf.Cos(tiltRadians), Mathf.Sin(tiltRadians));
            float forceMagnitude = isInWater ? 40f : 25f; // Faster sliding in water
            rb.AddForce(tiltDirection * forceMagnitude, ForceMode2D.Force);
        }

        // BOOST SPEED WHEN SLIDING ON GROUND (add this block)
        if (slide && isGrounded && !isInWater && rb != null) {
            float slideBoost = 1.3f; // Adjust as needed
            rb.velocity = new Vector2(rb.velocity.x * slideBoost, rb.velocity.y);
        }
        // BOOST SPEED WHEN SLIDING IN WATER (add this block)
        if (slide && isInWater && rb != null) {
            float waterSlideBoost = 1.36f; // Faster boost in water
            rb.velocity = new Vector2(rb.velocity.x * waterSlideBoost, rb.velocity.y);
        }

        jump = false;
        // Do not reset slide here; let Update handle it
    }

    // Add these methods at the end of your PlayerMovement class
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Water")) {
            isInWater = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Water")) {
            isInWater = false;
        }
    }
}
