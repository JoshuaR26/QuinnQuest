using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        Health playerHealth = collision.gameObject.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1);
        }
    }
}
