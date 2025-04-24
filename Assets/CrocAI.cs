using UnityEngine;

public class CrocAI : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float followSpeed = 3f;
    public float moveDistance = 5f;
    public float followRange = 8f;

    private Vector3 startPosition;
    private bool movingRight = true;
    private Transform player;

    private void Start()
    {
        startPosition = transform.position;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private void Update()
    {
        if (player != null && Vector3.Distance(transform.position, player.position) <= followRange)
        {
            FollowPlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        float speed = patrolSpeed;
        if (movingRight)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        if (Vector3.Distance(startPosition, transform.position) >= moveDistance)
        {
            Flip();
        }
    }

    void FollowPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * followSpeed * Time.deltaTime;

        if (direction.x != 0)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = Mathf.Sign(direction.x) * Mathf.Abs(localScale.x);
            transform.localScale = localScale;
        }
    }

    void Flip()
    {
        movingRight = !movingRight;
        transform.Rotate(0f, 180f, 0f); // Flip the sprite
    }
}
