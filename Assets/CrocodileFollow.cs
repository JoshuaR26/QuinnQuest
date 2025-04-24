using UnityEngine;

public class CrocodileFollow : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float followRange = 10f;
    private Transform player;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private void Update()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance <= followRange)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

                if (direction.x != 0)
                {
                    Vector3 localScale = transform.localScale;
                    localScale.x = Mathf.Sign(direction.x) * Mathf.Abs(localScale.x);
                    transform.localScale = localScale;
                }
            }
        }
    }
}