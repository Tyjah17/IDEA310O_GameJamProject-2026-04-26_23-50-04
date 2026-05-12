using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {

    [Header("Elevator")]
    public Transform elevatorObject;

    [Header("Movement")]
    public float moveDistance = 100f;
    public float moveSpeed = 5f;

    [Header("State")]
    public bool activated = false;

    private Vector3 targetPosition;

    void Start() {
        if (elevatorObject == null)
            elevatorObject = transform;
        targetPosition = elevatorObject.position + Vector3.down * moveDistance;
    }

    public string GetInteractText() {
        if (activated)
            return "Elevator Moving";
        return "Press E to use Elevator";
    }

    public void ActivateElevator() {
        if (activated)
            return;
        activated = true;
        StartCoroutine(MoveElevator());
    }

    IEnumerator MoveElevator() {
        while (Vector3.Distance(elevatorObject.position, targetPosition) > 0.05f) {
            elevatorObject.position = Vector3.MoveTowards(
                elevatorObject.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }
        elevatorObject.position = targetPosition;
    }
}