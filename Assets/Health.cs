using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 1;
    public int currentHealth;

    public HealthBarScript healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

    }
    void Update(){
        healthBar.SetHealth(currentHealth);
        if(currentHealth <= 0){
            Debug.Log("Game Over");
           //quit game code
        }
    }
    public void TakeDamage(int damage){
        currentHealth -= damage;
    }
}
