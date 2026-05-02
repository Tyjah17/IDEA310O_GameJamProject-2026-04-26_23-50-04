using UnityEngine;

public class GunADSFire : MonoBehaviour {

    [Header("References")]
    public Gun gunScript;
    public Transform gunObject;
    public Transform hipPosition;
    public Transform adsPosition;

    [Header("Movement")]
    public float moveSpeed = 10f;
    public float rotateSpeed = 10f;

    [Header("Aiming")]
    public float aimDistance = 100f;

    void LateUpdate() {
        if (gunScript == null || !gunScript.CanUseGun())
            return;

        HandleADSPosition();
        AimAtCrosshair();
    }

    void HandleADSPosition() {
        if (gunObject == null || hipPosition == null)
            return;

        bool isAiming = Input.GetMouseButton(1);

        Transform target = isAiming && adsPosition != null
            ? adsPosition
            : hipPosition;

        gunObject.localPosition = Vector3.Lerp(
            gunObject.localPosition,
            target.localPosition,
            Time.deltaTime * moveSpeed
        );
    }

    void AimAtCrosshair() {
        if (gunObject == null || Camera.main == null)
            return;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, aimDistance)) {
            targetPoint = hit.point;
        } else {
            targetPoint = ray.origin + ray.direction * aimDistance;
        }

        Quaternion targetRotation = Quaternion.LookRotation(
            targetPoint - gunObject.position
        );

        gunObject.rotation = Quaternion.Slerp(
            gunObject.rotation,
            targetRotation,
            Time.deltaTime * rotateSpeed
        );
    }
}