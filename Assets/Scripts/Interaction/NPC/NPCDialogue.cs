using System.Collections;
using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour {

    [Header("Dialogue")]
    [TextArea(3, 6)]
    public string[] dialogueLines;
    private int dialogueIndex = 0;
    public bool canRepeatLastLine = false;

    [Header("UI")]
    public TextMeshProUGUI dialogueUI;
    public float typeSpeed = 0.05f;

    [Header("Animator")]
    public Animator animator;
    public string talkingBoolName = "isTalking";

    [Header("Look At Player")]
    public Transform player;

    [Header("NPC Walk")]
    public NPCWalk npcWalker;
    public bool walkAfterFirstDialogue = true;

    private bool isTalking = false;
    private Coroutine typingRoutine;

    public string GetInteractText() {
        if (!HasDialogueLeft() || isTalking)
            return "";

        return "Press E to talk";
    }

    // talking methods
    public void Talk() {
        if (!HasDialogueLeft() || isTalking)
            return;
        isTalking = true;
        if (animator != null) {
            animator.SetBool(talkingBoolName, true);
        }
        // if repeat last line is enabled stay on final dialogue index
        int currentIndex = dialogueIndex;

        if (canRepeatLastLine && dialogueIndex >= dialogueLines.Length) {
            currentIndex = dialogueLines.Length - 1;
        }
        if (dialogueUI != null) {
            dialogueUI.gameObject.SetActive(true);

            if (typingRoutine != null) {
                StopCoroutine(typingRoutine);
            }

            typingRoutine = StartCoroutine(TypeDialogue(dialogueLines[currentIndex]));
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

        bool wasFirstDialogue = dialogueIndex == 0;
        bool isLastDialogue = dialogueIndex >= dialogueLines.Length - 1;

        if (wasFirstDialogue && !isLastDialogue && walkAfterFirstDialogue && npcWalker != null) {
            npcWalker.WalkToTarget();
        }

        dialogueIndex++;
    }

    bool HasDialogueLeft() {
        if (dialogueLines == null || dialogueLines.Length == 0)
            return false;

        if (dialogueIndex < dialogueLines.Length)
            return true;

        return canRepeatLastLine;
    }

    public bool IsTalking() {
        return isTalking;
    }

    IEnumerator TypeDialogue(string textToType) {
        string[] lines = textToType.Split('\n');

        foreach (string line in lines) {
            dialogueUI.text = "";

            foreach (char letter in line) {
                dialogueUI.text += letter;
                yield return new WaitForSeconds(typeSpeed);
            }

            yield return new WaitForSeconds(2f);
        }

        StopTalking();
    }

    // NPC head tranformation
    void OnAnimatorIK(int layerIndex) {

        if (animator == null || player == null)
            return;
        // get direction from NPC to player
        Vector3 directionToPlayer = player.position - transform.position;
        // remove vertical difference so angle check is only horizontal
        directionToPlayer.y = 0f;
        // calculate angle between where NPC is facing and player position
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        // get distance from NPC to player
        float distance = Vector3.Distance(transform.position, player.position);
        // default look weight
        float lookWeight = 1f;

        // if player is behind NPC stop head tracking completely
        if (angleToPlayer > 75f) {
            lookWeight = 0f;
        }
        // if player is too far away stop head tracking completely
        if (distance > 8f) {
            lookWeight = 0f;
        } else if (distance > 5f && lookWeight > 0f) {
            lookWeight = Mathf.Lerp(1f, 0f, (distance - 5f) / 3f);
        }

        Vector3 lookPosition = player.position + Vector3.up * 0.5f;

        animator.SetLookAtWeight(
            lookWeight,
            0f,
            lookWeight,
            0f,
            0f
        );

        animator.SetLookAtPosition(lookPosition);
    }
}