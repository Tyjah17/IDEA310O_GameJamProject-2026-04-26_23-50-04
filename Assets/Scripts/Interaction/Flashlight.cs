using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [Header("References")]
    public Hotbar hotbar;

    [Header("Held Flashlight")]
    public GameObject flashlightObject;
    public Light flashlightLight;

    [Header("Settings")]
    public string itemName = "Flashlight";
    public KeyCode toggleKey = KeyCode.F;

    private bool hasFlashlight = false;
    private bool isOn = false;

    void Start() {
        SetFlashlightVisible(false);
        SetLightEnabled(false);
    }

    void Update() {
        if (hotbar == null)
            return;

        UpdateHeldFlashlightVisibility();

        if (CanToggleFlashlight() && Input.GetKeyDown(toggleKey)) {
            ToggleFlashlight();
        }
    }

    public void UnlockFlashlight() {
        hasFlashlight = true;
    }

    public bool IsFlashlightOn() {
        return isOn;
    }

    bool CanToggleFlashlight() {
        if (!hasFlashlight)
            return false;

        string selectedItem = hotbar.GetSelectedItem();
        return selectedItem == itemName;
    }

    void ToggleFlashlight() {
        isOn = !isOn;
        SetLightEnabled(isOn);
    }

    void UpdateHeldFlashlightVisibility() {
        if (flashlightObject == null || hotbar == null)
            return;

        string selectedItem = hotbar.GetSelectedItem();
        bool shouldShow = hasFlashlight && selectedItem == itemName;

        SetFlashlightVisible(shouldShow);

        if (!shouldShow) {
            isOn = false;
            SetLightEnabled(false);
        }
    }

    void SetFlashlightVisible(bool value) {
        if (flashlightObject != null)
            flashlightObject.SetActive(value);
    }

    void SetLightEnabled(bool value) {
        if (flashlightLight != null)
            flashlightLight.enabled = value;
    }
}