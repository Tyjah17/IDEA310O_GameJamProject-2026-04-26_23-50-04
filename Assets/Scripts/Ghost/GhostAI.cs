using UnityEngine;
using UnityEngine.AI;

public class GhostAI : MonoBehaviour {
    [Header("References")]
    public Transform player;
    public NavMeshAgent agent;
    public Animator animator;
    private GhostVision vision;
    private GhostTeleport teleporter;

    [Header("Watch Disappear")]
    public float rotationSpeed = 0f;
    public float minWatchTime = 0f;
    public float maxWatchTime = 0f;
    private float currentWatchLimit;
    private float watchTimer = 0f;
    private bool isBeingWatched = false;

    [Header("Stun")]
    public float stunDuration = 3f;
    private bool isStunned = false;
    private float stunTimer = 0f;

    [Header("Audio")]
    public AudioSource ghostAudio;
    public float soundDistance = 10f;
    public float maxVolume = 1f;

    void Awake() {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
        if (animator == null)
            animator = GetComponent<Animator>();

        vision = GetComponent<GhostVision>();
        teleporter = GetComponent<GhostTeleport>();
    }

    void Start() {
        SetNewWatchTime();

        if (vision != null && vision.lookTarget == null) {
            vision.lookTarget = transform;
        }

        if (teleporter != null) {
            if (teleporter.agent == null)
                teleporter.agent = agent;
            if (teleporter.player == null)
                teleporter.player = player;
        }
    }

    void Update() {
        if (player == null || agent == null || vision == null || teleporter == null)
            return;

        if (isStunned) {
            agent.isStopped = true;

            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0) {
                isStunned = false;
                teleporter.TeleportToRandomSpawn();
                SetNewWatchTime();
            }

            UpdateAnimation();
            return;
        }

        isBeingWatched = vision.CanPlayerSeeGhost();
        agent.isStopped = false;
        agent.SetDestination(player.position);

        if (isBeingWatched) {
            RotateTowardPlayer();

            watchTimer += Time.deltaTime;
            if (watchTimer >= currentWatchLimit) {
                teleporter.TeleportToRandomSpawn();
                watchTimer = 0f;
                SetNewWatchTime();
                return;
            }
        } else {
            watchTimer = 0f;
        }
        UpdateAnimation();
        HandleSound();
    }

    void SetNewWatchTime() {
        currentWatchLimit = Random.Range(minWatchTime, maxWatchTime);
    }


    void RotateTowardPlayer() {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    void UpdateAnimation() {
        if (animator == null)
            return;
        bool isMoving = agent.velocity.magnitude > 0.1f;
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isIdle", !isMoving);
    }

    void HandleSound() {
        if (player == null || ghostAudio == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= soundDistance) {
            if (!ghostAudio.isPlaying)
                ghostAudio.Play();

            float volume = 1f - (distance / soundDistance);
            ghostAudio.volume = Mathf.Clamp(volume * maxVolume, 0f, maxVolume);
        } else {
            if (ghostAudio.isPlaying)
                ghostAudio.Stop();
        }
    }

    public void StunGhost() {
        StunGhost(stunDuration);
    }

    public void StunGhost(float duration) {
        isStunned = true;
        stunTimer = duration;
        watchTimer = 0f;

        if (agent != null) {
            agent.isStopped = true;
            agent.ResetPath();
        }
    }
}