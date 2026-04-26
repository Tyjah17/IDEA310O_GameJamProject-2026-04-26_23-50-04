using UnityEngine;
using TMPro;
using System.Collections;

public class FlashlightStun : MonoBehaviour {
    [Header("Flashlight")]
    public Light flashlight;
    public float stunRange = 0;
    public KeyCode stunKey = KeyCode.Mouse0;

    [Header("Flash Effect")]
    public float flashIntensity = 0;
    public float flashDuration = 0;

    [Header("Battery Charges")]
    public int maxFlashCharges = 3;
    public int currentFlashCharges = 3;
    public Batteries batteryUI;

    [Header("UI")]
    public TMP_Text batteryWarningText;
    public float messageDuration = 0;

    void Start() {
        currentFlashCharges = maxFlashCharges;
        UpdateBatteryUI();
    }

    void Update() {
        if (Input.GetKeyDown(stunKey)) {
            TryStunGhost();
        }
    }

    void TryStunGhost() {
        if (flashlight == null || !flashlight.enabled)
            return;
        if (currentFlashCharges <= 0){
            StartCoroutine(ShowBatteryWarning());
            return;
        }

        StartCoroutine(FlashEffect());
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, stunRange)) {
            GhostAI ghost = hit.collider.GetComponentInParent<GhostAI>();

            if (ghost != null) {
                ghost.StunGhost();
                currentFlashCharges--;
                UpdateBatteryUI();
            }
        }
    }

    void UpdateBatteryUI() {
        if (batteryUI == null)
            return;

        if (currentFlashCharges >= 3) {
            batteryUI.SetBatteryImage(BatteryStatus.Full);
        } else if (currentFlashCharges >= 2) {
            batteryUI.SetBatteryImage(BatteryStatus.Half);
        } else {
            batteryUI.SetBatteryImage(BatteryStatus.Empty);
        }
    }

    IEnumerator FlashEffect()
    {
        if (flashlight == null)
            yield break;
        float originalIntensity = flashlight.intensity;
        flashlight.intensity = flashIntensity;
        yield return new WaitForSeconds(flashDuration);
        flashlight.intensity = originalIntensity;
    }

    IEnumerator ShowBatteryWarning()
    {
        if (batteryWarningText == null)
            yield break;

        batteryWarningText.gameObject.SetActive(true);

        yield return new WaitForSeconds(messageDuration);

        batteryWarningText.gameObject.SetActive(false);
    }
}