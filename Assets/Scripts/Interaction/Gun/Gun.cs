using UnityEngine;

public class Gun : MonoBehaviour {

    [Header("References")]
    public Hotbar hotbar;
    public Camera playerCamera;
    public FirstPersonController firstPersonController;

    [Header("Gun/Light")]
    public GameObject gunObject;
    public Light gunLight;

    [Header("Settings")]
    public string itemName = "Gun";
    public KeyCode lightToggleKey = KeyCode.F;

    [Header("Zoom")]
    public float normalFOV = 75f;
    public float zoomFOV = 35f;
    public float zoomSpeed = 10f;

    [Header("Shooting")]
    public float shootDistance = 100f;
    public float fireRate = 0.25f;
    public int damage = 1;

    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 35f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip gunShotSound;

    private bool hasGun = false;
    private bool lightOn = false;
    private float nextFireTime = 0f;

    void Start() {
        SetGunVisible(false);
        SetLightEnabled(false);

        if (playerCamera != null) {
            playerCamera.fieldOfView = normalFOV;
        }
    }

    void Update() {
        if (hotbar == null)
            return;

        UpdateHeldGunVisibility();

        if (CanUseGun()) {
            HandleLight();
            HandleZoom();
            HandleShoot();
        } else {
            ResetZoom();
        }
    }

    public void UnlockGun() {
        hasGun = true;
    }

    public bool CanUseGun() {
        if (!hasGun)
            return false;

        return hotbar.GetSelectedItem() == itemName;
    }

    void HandleLight() {
        if (Input.GetKeyDown(lightToggleKey)) {
            lightOn = !lightOn;
            SetLightEnabled(lightOn);
        }
    }

    void HandleZoom() {
        if (playerCamera == null)
            return;

        bool isAiming = Input.GetMouseButton(1);

        if (firstPersonController != null) {
            firstPersonController.enableHeadBob = !isAiming;
        }

        float targetFOV = isAiming ? zoomFOV : normalFOV;

        playerCamera.fieldOfView = Mathf.Lerp(
            playerCamera.fieldOfView,
            targetFOV,
            Time.deltaTime * zoomSpeed
        );
    }

    void ResetZoom() {
        if (playerCamera == null)
            return;

        playerCamera.fieldOfView = Mathf.Lerp(
            playerCamera.fieldOfView,
            normalFOV,
            Time.deltaTime * zoomSpeed
        );

        if (firstPersonController != null) {
            firstPersonController.enableHeadBob = true;
        }
    }

    void HandleShoot() {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime) {
            nextFireTime = Time.time + fireRate;
            Shoot();
            ShootBullet();
            PlayGunSound();
        }
    }

    void Shoot() {
        if (playerCamera == null)
            return;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, shootDistance)) {
            Debug.Log("Shot hit: " + hit.collider.name);
        }
    }

    void ShootBullet() {
        if (bulletPrefab == null || bulletSpawnPoint == null || playerCamera == null)
            return;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, shootDistance, ~0, QueryTriggerInteraction.Ignore)) {
            targetPoint = hit.point;
        } else {
            targetPoint = ray.origin + ray.direction * shootDistance;
        }

        Vector3 shootDirection = (targetPoint - bulletSpawnPoint.position).normalized;

        GameObject bullet = Instantiate(
            bulletPrefab,
            bulletSpawnPoint.position,
            Quaternion.LookRotation(shootDirection)
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null) {
            rb.linearVelocity = shootDirection * bulletSpeed;
        }
    }

    void PlayGunSound() {
        if (audioSource != null && gunShotSound != null) {
            audioSource.PlayOneShot(gunShotSound);
        }
    }

    void UpdateHeldGunVisibility() {
        if (gunObject == null || hotbar == null)
            return;

        bool shouldShow = CanUseGun();

        SetGunVisible(shouldShow);

        if (!shouldShow) {
            lightOn = false;
            SetLightEnabled(false);
        }
    }

    void SetGunVisible(bool value) {
        if (gunObject != null)
            gunObject.SetActive(value);
    }

    void SetLightEnabled(bool value) {
        if (gunLight != null)
            gunLight.enabled = value;
    }
}