using UnityEngine;

public class GhostVision : MonoBehaviour {
    [Header("References")]
    public Camera playerCamera;
    public Transform lookTarget;

    [Header("Sight Check")]
    public float maxSightDistance = 0;
    public LayerMask visibilityMask = ~0;

    void Awake() {
        if (lookTarget == null)
            lookTarget = transform;
    }

    public bool CanPlayerSeeGhost() {
        if (playerCamera == null || lookTarget == null)
            return false;

        Vector3 targetPos = lookTarget.position;
        Vector3 viewportPoint = playerCamera.WorldToViewportPoint(targetPos);

        // behind camera
        if (viewportPoint.z <= 0f)
            return false;
        // outside camera view
        if (viewportPoint.x < 0f || viewportPoint.x > 1f || viewportPoint.y < 0f || viewportPoint.y > 1f)
            return false;

        Vector3 dir = targetPos - playerCamera.transform.position;
        float distance = dir.magnitude;

        if (distance > maxSightDistance)
            return false;

        Ray ray = new Ray(playerCamera.transform.position, dir.normalized);

        if (Physics.Raycast(ray, out RaycastHit hit, distance, visibilityMask)) {
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
                return true;
        }

        return false;
    }
}