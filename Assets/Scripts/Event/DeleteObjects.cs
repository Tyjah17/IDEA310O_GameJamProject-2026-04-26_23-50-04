using System.Collections.Generic;
using UnityEngine;

public class DeleteObjects : MonoBehaviour {

    [Header("Objects To Delete")]
    public List<GameObject> objectsToDelete = new List<GameObject>();

    [Header("Trigger Settings")]
    public bool triggerOnce = true;

    private bool triggered = false;

    void OnTriggerEnter(Collider other) {

        if (triggered && triggerOnce)
            return;

        if (other.CompareTag("Player")) {

            triggered = true;

            foreach (GameObject obj in objectsToDelete) {

                if (obj != null) {
                    Destroy(obj);
                }
            }
        }
    }
}