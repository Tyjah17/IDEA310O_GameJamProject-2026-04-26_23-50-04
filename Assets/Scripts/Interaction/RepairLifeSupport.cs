using UnityEngine;
using TMPro;

public class RepairLifeSupport : MonoBehaviour {

    [Header("State")]
    public bool repaired = false;

    [Header("Visuals")]
    public GameObject brokenEffects;
    public GameObject fixedEffects;

    public GameObject redLight;
    public GameObject greenLight;

    [Header("Command Center Screens")]
    public GameObject[] screensToTurnOff;
    public GameObject[] screensToTurnOn;

    [Header("UI")]
    public TextMeshProUGUI messageText;

    public string GetInteractText() {

        if (repaired)
            return "Life Support Online";

        return "Press E to restore Life Support";
    }

    public void Repair() {
        if (repaired)
            return;
        repaired = true;

        if (brokenEffects != null)
            brokenEffects.SetActive(false);
        if (fixedEffects != null)
            fixedEffects.SetActive(true);
        if (redLight != null)
            redLight.SetActive(false);
        if (greenLight != null)
            greenLight.SetActive(true);

        foreach (GameObject screen in screensToTurnOff) {
            if (screen != null)
                screen.SetActive(false);
        }
        foreach (GameObject screen in screensToTurnOn) {
            if (screen != null)
                screen.SetActive(true);
        }

        if (messageText != null) {
            messageText.text = "Life Support Restored";
            messageText.gameObject.SetActive(true);
        }
    }
}