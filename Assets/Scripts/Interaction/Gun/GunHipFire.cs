using UnityEngine;

public class GunHipFire : MonoBehaviour {

    [Header("References")]
    public Transform gunObject;
    public Transform hipPosition;

    [Header("Settings")]
    public float moveSpeed = 12f;
    public float aimDistance = 100f;

    void LateUpdate() {
        if (hipPosition == null || gunObject == null)
            return;

        // Keep gun in hip position
        gunObject.localPosition = Vector3.Lerp(
            gunObject.localPosition,
            hipPosition.localPosition,
            Time.deltaTime * moveSpeed
        );

        // Ray from crosshair (center of screen)
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, aimDistance)) {
            targetPoint = hit.point;
        } else {
            targetPoint = ray.origin + ray.direction * aimDistance;
        }

        // Aim gun toward crosshair
        Quaternion targetRotation = Quaternion.LookRotation(
            targetPoint - gunObject.position
        );

        gunObject.rotation = Quaternion.Slerp(
            gunObject.rotation,
            targetRotation,
            Time.deltaTime * moveSpeed
        );
    }
}