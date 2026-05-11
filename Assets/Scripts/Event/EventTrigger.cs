using System.Collections;
using UnityEngine;

public class EventTrigger : MonoBehaviour {

    [Header("Target")]
    public GameObject targetObject;
    public Animator targetAnimator;
    public EventMover mover;

    [Header("Animation")]
    public string triggerName = "PlayWalk";

    [Header("Timing")]
    public float delayBeforePlay = 0f;
    public float duration = 4f;

    [Header("Options")]
    public bool disableAfter = true;
    public bool triggerOnce = true;

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other) {
        if (triggerOnce && hasTriggered) return;

        if (other.CompareTag("Player")) {
            hasTriggered = true;
            StartCoroutine(HandleEvent());
        }
    }

    IEnumerator HandleEvent() {

        yield return new WaitForSeconds(delayBeforePlay);

        if (targetObject != null) {
            targetObject.SetActive(true);
        }

        if (targetAnimator != null) {
            targetAnimator.SetTrigger(triggerName);
        }

        if (mover != null) {
            mover.StartMove();
        }

        yield return new WaitForSeconds(duration);

        if (disableAfter && targetObject != null) {
            targetObject.SetActive(false);
        }
    }
}