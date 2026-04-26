using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {
    [Header("Health")]
    public int maxHealth = 3;
    public int health;

    [Header("Heart UI")]
    public Hearts[] hearts;

    [Header("GameOver UI")]
    public GameObject gameOverPanel;
    public GameUIManager gameUI;

    void Start() {
        health = maxHealth;
        UpdateHearts();
    }

    public void TakeDamage(int damage) {
        health -= damage;
        UpdateHearts();
        if (health <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        health += amount;

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        UpdateHearts();
    }

    void UpdateHearts() {
        for (int i = 0; i < hearts.Length; i++) {
            if (i < health){
                hearts[i].SetHeartImage(HeartStatus.Full);
            } else {
                hearts[i].SetHeartImage(HeartStatus.Empty);
            }
        }
    }

    void Die() {
        gameUI.ShowGameOver();
    }
}