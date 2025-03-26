using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocoMovement : MonoBehaviour {
    public float speed = 2f; // Movement speed
    public float moveDistance = 5f; // Distance before turning
    private Vector3 startPosition;
    private bool movingRight = true;

    void Start() {
        startPosition = transform.position;
    }

    void Update() {
        MoveCrocodile();
    }

    void MoveCrocodile() {
        if (movingRight) {
            transform.position += Vector3.left * speed * Time.deltaTime;
        } else {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        if (Vector3.Distance(startPosition, transform.position) >= moveDistance) {
            Flip();
        }
    }

    void Flip() {
        movingRight = !movingRight;
        transform.Rotate(0f, 180f, 0f); // Rotate the crocodile to face the other direction
    }
}
