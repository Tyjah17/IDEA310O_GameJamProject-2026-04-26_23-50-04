using UnityEngine;
using TMPro;

public class RepairLifeSupport : MonoBehaviour {

    [Header("State")]
    public bool repaired = false;
    public bool crashEventHappened = false;

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

    [Header("Audio")]
    public AudioSource alarmAudioSource;
    public AudioSource repairAudioSource;
    public AudioClip repairSound;
    public AudioClip damageSound;

    public string GetInteractText() {

        if (repaired)
            return "Life Support Online";

        return "Press E to restore Life Support";
    }

    void Update() {
        HandleDamagedAlarm();
    }

    void HandleDamagedAlarm() {
        if (alarmAudioSource == null || damageSound == null)
            return;

        if (repaired) {
            if (alarmAudioSource.isPlaying) {
                alarmAudioSource.Stop();
            }
            return;
        }

        if (crashEventHappened && !alarmAudioSource.isPlaying) {
            alarmAudioSource.clip = damageSound;
            alarmAudioSource.loop = true;
            alarmAudioSource.Play();
        }
    }

    public void StartCrashEvent() {
        crashEventHappened = true;
    }

    public void Repair() {
        if (repaired)
            return;
        repaired = true;
        // stop alarm sound
        if (alarmAudioSource != null) {
            alarmAudioSource.Stop();
            alarmAudioSource.loop = false;
        }
        // play repair sound
        if (repairAudioSource != null && repairSound != null) {
            repairAudioSource.clip = repairSound;
            repairAudioSource.Play();
            repairAudioSource.loop = true;
        }
        // effects
        if (brokenEffects != null)
            brokenEffects.SetActive(false);
        if (fixedEffects != null)
            fixedEffects.SetActive(true);
        // lights
        if (redLight != null)
            redLight.SetActive(false);
        if (greenLight != null)
            greenLight.SetActive(true);
        // screens
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