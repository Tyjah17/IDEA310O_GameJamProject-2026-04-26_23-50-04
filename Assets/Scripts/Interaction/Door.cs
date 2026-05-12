using UnityEngine;

public class Door : MonoBehaviour {

    [Header("Door")]
    public bool isOpen = false;

    [Header("Key Requirement")]
    public string requiredKeyId = "KeyA";

    [Header("Object To Open")]
    public GameObject objectToOpen;

    public string GetInteractText(Hotbar hotbar) {
        if (isOpen)
            return "Gate Activated";
        bool hasKey = hotbar != null && hotbar.HasUsableKey(requiredKeyId);
        if (hasKey)
            return "Press E to Activate";
        return "Need Keycard: " + requiredKeyId;
    }

    public void Interact(Hotbar hotbar) {
        if (isOpen)
            return;
        bool hasKey = hotbar != null && hotbar.HasUsableKey(requiredKeyId);
        if (!hasKey)
            return;
        if (hotbar.UseKey(requiredKeyId)) {
            OpenDoor();
        }
    }

    void OpenDoor() {
        isOpen = true;
        if (objectToOpen != null) {
            objectToOpen.SetActive(false);
        }
    }
}