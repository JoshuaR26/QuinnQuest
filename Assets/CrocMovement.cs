using UnityEngine;

public class CrocMovement : MonoBehaviour {
    public float speed = 2f; // Movement speed
    public float moveDistance = 5f; // Distance before turning
    private Vector3 startPosition;
    private bool movingRight = true;

    void Start() {
        startPosition = transform.position; // Store initial position
    }

    void Update() {
        MoveCrocodile(); // Move crocodile every frame
    }

    void MoveCrocodile() {
        // Move horizontally based on direction
        float moveStep = speed * Time.deltaTime * (movingRight ? 1 : -1);
        transform.position += new Vector3(moveStep, 0, 0);

        // Debugging: Check position and movement
        Debug.Log($"Moving Right: {movingRight} | Position: {transform.position}");

        // Flip when reaching move distance
        if (Vector3.Distance(startPosition, transform.position) >= moveDistance) {
            Flip();
        }
    }

    void Flip() {
        movingRight = !movingRight; // Toggle direction

        // Flip using localScale (avoids pivot issues)
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Invert X axis
        transform.localScale = scale;
    }
}
