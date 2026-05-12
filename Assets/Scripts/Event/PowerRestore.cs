using UnityEngine;
using System.Collections;

public class PowerRestore : MonoBehaviour {

    [Header("Lights Parent")]
    public GameObject lightsParent;

    [Header("Restore Flicker")]
    public bool flicker = true;
    public float flickerTime = 2f;

    private bool powerRestored = false;
    private Light[] allLights;

    void Start() {
        allLights = lightsParent.GetComponentsInChildren<Light>();
    }

    public void RestorePower() {
        if (powerRestored)
            return;
        powerRestored = true;
        if (flicker) {
            StartCoroutine(FlickerLightsOn());
        } else {
            TurnLightsOn();
        }
    }

    IEnumerator FlickerLightsOn() {
        float timer = 0f;
        while (timer < flickerTime) {
            ToggleLights();
            timer += 0.1f;
            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        }
        TurnLightsOn();
    }

    // toggle on the light not the object
    void ToggleLights() {
        foreach (Light lightObject in allLights) {
            lightObject.enabled = !lightObject.enabled;
        }
    }

    void TurnLightsOn() {
        foreach (Light lightObject in allLights) {
            lightObject.enabled = true;
        }
    }

    public bool IsPowerRestored() {
        return powerRestored;
    }
}