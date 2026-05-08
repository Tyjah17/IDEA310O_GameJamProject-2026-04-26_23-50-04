using System.Collections;
using UnityEngine;

public class EventMover : MonoBehaviour {

    [Header("Path")]
    public Transform startPoint;
    public Transform endPoint;

    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Rotation")]
    public bool faceMoveDirection = true;
    public Vector3 rotationOffset = Vector3.zero;

    public void StartMove() {
        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine() {
        transform.position = startPoint.position;

        while (Vector3.Distance(transform.position, endPoint.position) > 0.1f) {
            Vector3 direction = (endPoint.position - transform.position).normalized;

            if (faceMoveDirection && direction != Vector3.zero) {
                Quaternion faceRotation = Quaternion.LookRotation(direction);
                transform.rotation = faceRotation * Quaternion.Euler(rotationOffset);
            }

            transform.position = Vector3.MoveTowards(
                transform.position,
                endPoint.position,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }
    }
}