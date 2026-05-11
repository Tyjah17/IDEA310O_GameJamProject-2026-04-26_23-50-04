using UnityEngine;

public class AlienHealth : MonoBehaviour {
    [Header("Health")]
    public int maxHealth = 3;

    private int currentHealth;
    private Animator anim;

    void Start() {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        Destroy(gameObject);
    }
}