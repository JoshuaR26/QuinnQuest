using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;

    public HealthBarScript healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        // if (currentHealth <= 0)
        // {
            // Game ovr
        // }
    }
}
