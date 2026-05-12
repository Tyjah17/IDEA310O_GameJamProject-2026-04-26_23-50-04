using UnityEngine;

public class Door : MonoBehaviour {

    [Header("Door")]
    public bool isOpen = false;

    [Header("Key Requirement")]
    public string requiredKeyId = "KeyA";

    [Header("Object To Open")]
    public GameObject objectToOpen;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip deniedSound;

    public string GetInteractText(Hotbar hotbar) {
        if (isOpen)
            return "Gate Activated";
        bool hasKey = hotbar != null && hotbar.HasUsableKey(requiredKeyId);
        if (hasKey)
            return "Press E to Activate";
        return "Need Keycard: " + requiredKeyId + " to Activate";
    }

    public void Interact(Hotbar hotbar) {
        if (isOpen)
            return;

        bool hasKey = hotbar != null && hotbar.HasUsableKey(requiredKeyId);
        if (!hasKey) {
            if (audioSource != null && deniedSound != null) {
                audioSource.PlayOneShot(deniedSound);
            }
            return;
        }

        bool usedKey = hotbar.UseKey(requiredKeyId);
        if (usedKey) {
            isOpen = true;
            if (audioSource != null && openSound != null) {
                audioSource.PlayOneShot(openSound);
            }
            if (objectToOpen != null) {
                objectToOpen.SetActive(false);
            }
        }
    }
}