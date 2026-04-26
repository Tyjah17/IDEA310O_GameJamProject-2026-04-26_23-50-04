using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public enum ItemType {
        Generic,
        Flashlight,
        Key,
        Heart,
        Battery
    }

    [Header("Item Info")]
    public string itemName = "Item";
    public ItemType itemType = ItemType.Generic;
    public bool destroyOnPickup = true;

    [Header("Key Settings")]
    public string keyId = "";
    public int keyUses = 1;

    [Header("UI")]
    public GameObject batteryHUD;

    private PlayerHealth player;

    public void OnPickup(Hotbar hotbar, Flashlight flashlight)
    {
        switch (itemType)
        {
            case ItemType.Key:
                if (hotbar != null)
                {
                    hotbar.AddKey(keyId, keyUses, itemName);
                }
                break;

            case ItemType.Flashlight:
                if (hotbar != null)
                {
                    hotbar.AddItem(itemName);
                }

                if (flashlight != null)
                {
                    flashlight.UnlockFlashlight();
                }

                if (batteryHUD != null)
                {
                    batteryHUD.SetActive(true);
                }
                break;
            case ItemType.Heart:
                PlayerHealth health = player.GetComponent<PlayerHealth>();

                if (health != null && health.health < health.maxHealth)
                {
                    health.Heal(1);
                    Destroy(gameObject);
                }
                break;

            default:
                if (hotbar != null)
                {
                    hotbar.AddItem(itemName);
                }
                break;
        }

        if (destroyOnPickup) {
            Destroy(gameObject);
        } else {
            gameObject.SetActive(false);
        }
    }
}