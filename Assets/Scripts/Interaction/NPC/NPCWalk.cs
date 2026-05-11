using UnityEngine;
using UnityEngine.AI;

public class NPCWalk : MonoBehaviour {

    [Header("Movement")]
    public NavMeshAgent agent;
    public Transform walkTarget;

    [Header("Adjust Rotation")]
    public Transform adjustment;
    public float rotateSpeed = 0f;

    [Header("Animation")]
    public Animator animator;
    public string walkingBoolName = "isWalking"; 

    private bool isWalking = false;
    private bool rotate = false;

    void Awake() {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void WalkToTarget() {
        if (agent == null || walkTarget == null)
            return;

        isWalking = true;
        rotate = true;

        agent.updateRotation = true;
        agent.isStopped = false;
        agent.SetDestination(walkTarget.position);
    }

    void Update() {
        if (agent == null)
            return;
        // check if npc id moving
        bool isMoving = agent.velocity.magnitude > 0.1f;
        // update animation
        if (animator != null) {
            animator.SetBool(walkingBoolName, isMoving);
        }
        // arrived at destination
        if (isWalking && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) {
            isWalking = false;
            agent.isStopped = true;
            agent.ResetPath();
        }
        // adjust npc rotation
        if (!isWalking && rotate && adjustment != null) {
            RotateAfterArriving();
        }
    }

    void RotateAfterArriving() {
        // get direction toward adjustment object
        Vector3 direction = adjustment.position - transform.position;
        // keep rotation horizontal only
        direction.y = 0f;
        if (direction == Vector3.zero)
            return;
        // calculate target rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        // smoothly rotate NPC
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotateSpeed * Time.deltaTime
        );
        // stop rotating once close enough
        if (Quaternion.Angle(transform.rotation, targetRotation) < 2f) {

            transform.rotation = targetRotation;

            rotate = false;
        }
    }
}