using UnityEngine;

public class ElevatorConsole : MonoBehaviour {

    [Header("State")]
    public bool activated = false;

    [Header("Power Requirement")]
    public PowerRestore powerRestore;

    [Header("Object To Open")]
    public GameObject objectToOpen;

    public string GetInteractText() {
        if (activated)
            return "Elevator Console Online";
        if (powerRestore == null || !powerRestore.IsPowerRestored())
            return "Elevator Console Needs Power";
        return "Press E to activate Elevator Console";
    }

    public void ActivateConsole() {
        if (activated)
            return;
        if (powerRestore == null || !powerRestore.IsPowerRestored())
            return;
        activated = true;
        if (objectToOpen != null)
            objectToOpen.SetActive(false);
    }
}