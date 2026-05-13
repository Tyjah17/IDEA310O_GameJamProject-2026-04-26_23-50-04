using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour {

    [Header("References")]
    public Transform player;
    public NavMeshAgent agent;
    public Animator animator;

    [Header("Combat")]
    public int health = 10;
    public float chaseRange = 20f;
    public float attackRange = 2.5f;
    public float attackCooldown = 2f;

    [Header("Damage")]
    public int attackDamage = 1;
    public float damageDelay = 0.5f;
    private bool damageQueued = false;

    [Header("Dodge")]
    public float dodgeChance = 0.3f;
    private bool dodging = false;

    [Header("Loot")]
    public GameObject itemDropPrefab;
    public Transform dropPoint;

    private float nextAttackTime = 0f;
    private bool dead = false;

    void Update() {
        if (dead || player == null || agent == null || animator == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange) {
            agent.isStopped = true;
            agent.ResetPath();

            animator.SetBool("isMoving", false);
            animator.SetBool("isRunning", false);

            FacePlayer();

            if (Time.time >= nextAttackTime) {
                animator.ResetTrigger("attack");
                animator.SetTrigger("attack");
                if (!damageQueued) {
                    StartCoroutine(DealDamageAfterDelay());
                }
                nextAttackTime = Time.time + attackCooldown;
            }

            return;
        }

        if (distance <= chaseRange) {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            animator.SetBool("isMoving", true);
            animator.SetBool("isRunning", true);
            return;
        }

        agent.isStopped = true;
        agent.ResetPath();

        animator.SetBool("isMoving", false);
        animator.SetBool("isRunning", false);
    }

    public void TakeDamage(int damage) {
        if (dead || dodging)
            return;
        // random dodgee
        if (Random.value <= dodgeChance) {
            dodging = true;
            animator.SetTrigger("dodge");
            StartCoroutine(EndDodge());
            return;
        }
        health -= damage;
        animator.SetTrigger("getHit");
        if (health <= 0) {
            Die();
        }
    }

    void Die() {
        dead = true;

        if (agent != null) {
            agent.isStopped = true;
            agent.enabled = false;
        }

        animator.SetBool("isDead", true);

        if (itemDropPrefab != null) {
            Vector3 spawnPosition = dropPoint != null ? dropPoint.position : transform.position + Vector3.up * 1f;

            GameObject droppedItem = Instantiate(
                itemDropPrefab,
                spawnPosition,
                itemDropPrefab.transform.rotation
            );

            droppedItem.SetActive(true);
        }
    }

    void FacePlayer() {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    System.Collections.IEnumerator DealDamageAfterDelay() {
        damageQueued = true;

        yield return new WaitForSeconds(damageDelay);

        if (!dead && player != null) {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= attackRange + 0.5f) {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

                if (playerHealth != null) {
                    playerHealth.TakeDamage(attackDamage);
                }
            }
        }

        damageQueued = false;
    }

    System.Collections.IEnumerator EndDodge() {

        yield return new WaitForSeconds(1f);

        dodging = false;
    }
}