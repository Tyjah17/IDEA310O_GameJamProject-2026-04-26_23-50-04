using UnityEngine;

public class Damage : MonoBehaviour {
    public int damageAmount = 1;
    public float damageRate = 1f;

    private float damageTimer = 0f;
    private bool hasDealtInitialDamage = false;

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
            if (playerHealth == null)
                return;

            if (!hasDealtInitialDamage) {
                playerHealth.TakeDamage(damageAmount);
                hasDealtInitialDamage = true;
                damageTimer = 0f;
                return;
            }

            damageTimer += Time.deltaTime;
            if (damageTimer >= 1f / damageRate) {
                playerHealth.TakeDamage(damageAmount);
                damageTimer = 0f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            damageTimer = 0f;
        hasDealtInitialDamage = false;
    }
}