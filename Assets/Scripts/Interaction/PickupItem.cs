using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public enum ItemType {
        Generic,
        Flashlight,
        Key,
        Gun,
        PowerCell
    }

    [Header("Item Info")]
    public string itemName = "Item";
    public ItemType itemType = ItemType.Generic;
    public bool destroyOnPickup = true;

    [Header("Key Settings")]
    public string keyId = "";
    public int keyUses = 1;

    [Header("Collectable Settings")]
    public int collectableAmount = 1;

    [Header("UI")]
    public GameObject batteryHUD;

    private PlayerHealth player;

    public void OnPickup(Hotbar hotbar, Flashlight flashlight, Gun gun)
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
            case ItemType.Gun:
                if (hotbar != null)
                {
                    hotbar.AddItem(itemName);
                }

                if (gun != null)
                {
                    gun.UnlockGun();
                }
                break;
            case ItemType.PowerCell:
                if (hotbar != null) {
                    hotbar.AddStackableItem("Power Cell", collectableAmount);
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