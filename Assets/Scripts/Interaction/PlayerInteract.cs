using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public Hotbar hotbar;
    public Flashlight flashlight;

    [Header("Interaction Settings")]
    public float interactDistance = 3f;
    public KeyCode interactKey = KeyCode.E;

    [Header("UI")]
    public TextMeshProUGUI interactText;

    void Update() {
        CheckForInteractable();
    }

    void CheckForInteractable() {
        if (playerCamera == null) {
            HideInteractText();
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (!Physics.Raycast(ray, out RaycastHit hit, interactDistance)) {
            HideInteractText();
            return;
        }
        PickupItem pickup = hit.collider.GetComponentInParent<PickupItem>();
        if (pickup != null)
        {
            HandlePickup(pickup);
            return;
        }
        Door door = hit.collider.GetComponentInParent<Door>();
        if (door != null) {
            HandleDoor(door);
            return;
        }
        LevelExit levelExit = hit.collider.GetComponentInParent<LevelExit>();
        if (levelExit != null) {
            HandleLevelExit(levelExit);
            return;
        }
        HideInteractText();
    }

    void HandlePickup(PickupItem pickup) {
        ShowInteractText("Press E to pick up " + pickup.itemName);

        if (Input.GetKeyDown(interactKey)) {
            pickup.OnPickup(hotbar, flashlight);
        }
    }

    void HandleDoor(Door door) {
        bool hasKey = hotbar != null && hotbar.HasUsableKey(door.requiredKeyId);
        int usesLeft = hotbar != null ? hotbar.GetKeyUses(door.requiredKeyId) : 0;

        if (hasKey) {
            ShowInteractText("Press E to open door (" + usesLeft + " use left)");
        } else {
            ShowInteractText("Need key: " + door.requiredKeyId);
        }

        if (Input.GetKeyDown(interactKey) && hasKey) {
            if (hotbar.UseKey(door.requiredKeyId)) {
                door.OpenDoor();
            }
        }
    }

    void HandleLevelExit(LevelExit levelExit) {
        ShowInteractText("Press E to exit level");

        if (Input.GetKeyDown(interactKey)) {
            levelExit.Interact();
        }
    }

    void ShowInteractText(string message) {
        if (interactText == null)
            return;

        interactText.text = message;
        interactText.gameObject.SetActive(true);
    }

    void HideInteractText() {
        if (interactText != null) {
            interactText.gameObject.SetActive(false);
        }
    }
}