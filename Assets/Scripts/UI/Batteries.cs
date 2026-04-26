using UnityEngine;
using UnityEngine.UI;

public enum BatteryStatus
{
    Empty = 0,
    Half = 1,
    Full = 2
}

public class Batteries : MonoBehaviour {
    public Sprite emptyBattery, halfBattery, fullBattery;
    Image batteryImage;

    private void Awake() {
        batteryImage = GetComponent<Image>();
    }

    public void SetBatteryImage(BatteryStatus status) {
        switch (status) {
            case BatteryStatus.Empty:
                batteryImage.sprite = emptyBattery;
                break;
            case BatteryStatus.Half:
                batteryImage.sprite = halfBattery;
                break;
            case BatteryStatus.Full:
                batteryImage.sprite = fullBattery;
                break;
        }
    }
}
