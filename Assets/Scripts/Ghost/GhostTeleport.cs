using UnityEngine;
using UnityEngine.AI;

public class GhostTeleport : MonoBehaviour {
    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("References")]
    public NavMeshAgent agent;
    public Transform player;

    void Awake() {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
    }

    public void TeleportToRandomSpawn() {
        if (spawnPoints == null || spawnPoints.Length == 0) {
            return;
        }

        if (agent == null) {
            return;
        }

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        agent.Warp(spawnPoint.position);

        if (player != null) {
            Vector3 direction = player.position - transform.position;
            direction.y = 0f;

            if (direction != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(direction);
        }
        agent.isStopped = false;
    }
}