using UnityEngine;

public class Door : MonoBehaviour {
    [Header("Door")]
    public bool isOpen = false;

    [Header("Key Requirement")]
    public string requiredKeyId = "KeyA";

    public bool RequiresKey() {
        return !string.IsNullOrEmpty(requiredKeyId);
    }

    public void OpenDoor() {
        if (isOpen)
            return;
        isOpen = true;
        gameObject.SetActive(false);
    }
}