using UnityEngine;

public class Door : MonoBehaviour {
    [Header("Door")]
    public bool isOpen = false;

    [Header("Key Requirement")]
    public string requiredKeyId = "KeyA";

    [Header("Object To Open")]
    public GameObject objectToOpen;

    public bool RequiresKey() {
        return !string.IsNullOrEmpty(requiredKeyId);
    }

    public void OpenDoor() {
        if (isOpen)
            return;
        isOpen = true;
        objectToOpen.SetActive(false);
    }
}