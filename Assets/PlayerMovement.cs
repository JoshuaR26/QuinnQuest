using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public CharacterController2D controller; 
    public Animator animator;
    public Rigidbody2D rb; // Add this line to reference the Rigidbody2D

    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    bool slide = false;

    void Awake() {
        // Get the Rigidbody2D component if not set in Inspector
        if (rb == null) rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        float currentSpeed = animator.GetFloat("Speed");

        if (Input.GetButtonDown("Jump")) {
            jump = true;
            animator.SetBool("isJumping", true);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && currentSpeed > 0.1f) {
            if(animator.GetBool("isSliding")){
                rb.freezeRotation = false;
            }
            slide = true;
            animator.SetBool("isSliding", true);
            // Unfreeze Z rotation while sliding
            rb.constraints &= ~RigidbodyConstraints2D.FreezeRotation;

        } else if (Input.GetKeyUp(KeyCode.LeftShift) || currentSpeed <= 0.01f) {
            slide = false;
            animator.SetBool("isSliding", false);
            // Freeze Z rotation when not sliding
            rb.freezeRotation = true;
            rb.constraints |= RigidbodyConstraints2D.FreezeRotation;
        }
    }
    
    public void OnLanding() {
        animator.SetBool("isJumping", false);
    }

    void FixedUpdate() {
        controller.Move(horizontalMove * Time.fixedDeltaTime, slide, jump);
        jump = false; 
        slide = false;
    }
}
