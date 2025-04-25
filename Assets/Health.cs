using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int maxHealth = 1;
    public int currentHealth;

    public HealthBarScript healthBar;
    public GameObject gameOverPanel; // Assign in inspector

    private bool isGameOver = false;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        gameOverPanel.SetActive(false); // Hide Game Over at start
    }

    void Update()
    {
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0 && !isGameOver)
        {
            GameOver();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f; // Freeze the game
        gameOverPanel.SetActive(true); // Show Game Over UI
        Debug.Log("Game Over");
    }
}
