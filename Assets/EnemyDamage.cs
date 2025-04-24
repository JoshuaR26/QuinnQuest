using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        Health playerHealth = other.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1);
        }
    }
}
