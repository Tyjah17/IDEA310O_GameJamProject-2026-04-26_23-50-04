using UnityEngine;
using System.Collections;

public class PowerOutage : MonoBehaviour {

    [Header("Lights Parent")]
    public GameObject lightsParent;

    [Header("Flicker Before Shutdown")]
    public bool flicker = true;
    public float flickerTime = 2f;

    private bool outageTriggered = false;

    private Light[] allLights;

    void Start() {
        allLights = lightsParent.GetComponentsInChildren<Light>();
    }

    public void TriggerOutage() {
        if (outageTriggered)
            return;
        outageTriggered = true;
        if (flicker) {
            StartCoroutine(FlickerLights());
        } else {
            TurnLightsOff();
        }
    }

    IEnumerator FlickerLights() {
        float timer = 0f;
        while (timer < flickerTime) {
            ToggleLights();
            timer += 0.1f;
            yield return new WaitForSeconds(
                Random.Range(0.05f, 0.2f)
            );
        }
        TurnLightsOff();
    }

    void ToggleLights() {
        foreach (Light lightObject in allLights) {
            lightObject.enabled = !lightObject.enabled;
        }
    }

    void TurnLightsOff() {
        foreach (Light lightObject in allLights) {
            lightObject.enabled = false;
        }
    }
}