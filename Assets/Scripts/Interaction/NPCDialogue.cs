using System.Collections;
using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour {

    [Header("Dialogue")]
    [TextArea(3, 6)]
    public string dialogueText = "";

    [Header("UI")]
    public TextMeshProUGUI dialogueUI;
    public float typeSpeed = 0.03f;

    [Header("Animator")]
    public Animator animator;
    public string talkingBoolName = "isTalking";
    private bool isTalking = false;

    private Coroutine typingRoutine;

    public string GetInteractText() {
        return "Press E to talk";
    }

    public void Talk() {
        isTalking = true;
        if (animator != null) {
            animator.SetBool(talkingBoolName, true);
        }

        if (dialogueUI != null) {
            dialogueUI.gameObject.SetActive(true);
            if (typingRoutine != null) {
                StopCoroutine(typingRoutine);
            }
            typingRoutine = StartCoroutine(TypeDialogue());
        }
    }

    public void StopTalking() {
        isTalking = false;

        if (animator != null) {
            animator.SetBool(talkingBoolName, false);
        }
        if (dialogueUI != null) {
            dialogueUI.gameObject.SetActive(false);
        }
    }

    IEnumerator TypeDialogue() {
        dialogueUI.text = "";

        foreach (char letter in dialogueText) {
            dialogueUI.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }

        yield return new WaitForSeconds(2f);
        StopTalking();
    }

    public bool IsTalking() {
        return isTalking;
    }
}