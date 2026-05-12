using UnityEngine;
using TMPro;

// Handles all Player Interaction throughout games

public class PlayerInteract : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public Hotbar hotbar;
    public Flashlight flashlight;
    public Gun gun;

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
        // item interaction
        PickupItem pickup = hit.collider.GetComponentInParent<PickupItem>();
        if (pickup != null)
        {
            HandlePickup(pickup);
            return;
        }
        // door interation / console interaction
        Door door = hit.collider.GetComponentInParent<Door>();
        if (door != null) {
            ShowInteractText(door.GetInteractText(hotbar));
            if (Input.GetKeyDown(interactKey)) {
                door.Interact(hotbar);
            }
            return;
        }
        LevelExit levelExit = hit.collider.GetComponentInParent<LevelExit>();
        if (levelExit != null) {
            HandleLevelExit(levelExit);
            return;
        }
        // NPC interaction
        NPCDialogue npc = hit.collider.GetComponentInParent<NPCDialogue>();
        if (npc != null) {
            if (!npc.IsTalking()) {
                ShowInteractText(npc.GetInteractText());
            } else {
                HideInteractText();
            }
            if (Input.GetKeyDown(interactKey)) {
                npc.Talk();
                HideInteractText();
            }

            return;
        }
        // life support interaction
        RepairLifeSupport lifeSupport = hit.collider.GetComponentInParent<RepairLifeSupport>();
        if (lifeSupport != null) {
            ShowInteractText(lifeSupport.GetInteractText());
            if (Input.GetKeyDown(interactKey)) {
                lifeSupport.Repair();
            }
            return;
        }
        // power cell interaction
        PowerCell powerCell = hit.collider.GetComponentInParent<PowerCell>();
        if (powerCell != null) {
            ShowInteractText(powerCell.GetInteractText(hotbar));

            if (Input.GetKeyDown(interactKey)) {
                powerCell.InstallPowerCell(hotbar);
            }

            return;
        }
        // elevator console interaction
        ElevatorConsole elevatorConsole = hit.collider.GetComponentInParent<ElevatorConsole>();
        if (elevatorConsole != null) {
            ShowInteractText(elevatorConsole.GetInteractText());
            if (Input.GetKeyDown(interactKey)) {
                elevatorConsole.ActivateConsole();
            }
            return;
        }
        // elevator interaction
        Elevator elevator = hit.collider.GetComponentInParent<Elevator>();
        if (elevator != null) {
            ShowInteractText(elevator.GetInteractText());
            if (Input.GetKeyDown(interactKey)) {
                elevator.ActivateElevator();
            }
            return;
        }
        HideInteractText();
    }

    void HandlePickup(PickupItem pickup) {
        ShowInteractText("Press E to Pick Up " + pickup.itemName);

        if (Input.GetKeyDown(interactKey)) {
            pickup.OnPickup(hotbar, flashlight, gun);
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