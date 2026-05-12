using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hotbar : MonoBehaviour {
    [System.Serializable]
    public class KeyData
    {
        public string keyId;
        public string displayName;
        public int uses;
    }

    [System.Serializable]
    public class StackItemData {
        public string itemName;
        public int amount;
    }

    public List<string> items = new List<string>();
    public List<KeyData> keys = new List<KeyData>();
    public List<StackItemData> stackItems = new List<StackItemData>();

    public TextMeshProUGUI[] slotTexts;
    public Image[] slotBorders;

    public int selectedSlot = 0;

    void Start() {
        UpdateHotbar();
        HighlightSelectedSlot();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectSlot(4);
    }
    // normal item
    public void AddItem(string itemName) {
        if (items.Count < slotTexts.Length) {
            items.Add(itemName);
            UpdateHotbar();
        }
    }
    // stackable item methods
    public void AddStackableItem(string itemName, int amount) {

        StackItemData existingItem = stackItems.Find(i => i.itemName == itemName);

        if (existingItem != null) {
            existingItem.amount += amount;
        } else {

            StackItemData newItem = new StackItemData();
            newItem.itemName = itemName;
            newItem.amount = amount;

            stackItems.Add(newItem);
        }

        UpdateHotbar();
    }

    public int GetStackItemAmount(string itemName) {
        StackItemData item = stackItems.Find(i => i.itemName == itemName);
        return item != null ? item.amount : 0;
    }

    public bool UseStackItem(string itemName, int amount) {
        StackItemData item = stackItems.Find(i => i.itemName == itemName);

        if (item == null || item.amount < amount)
            return false;

        item.amount -= amount;

        if (item.amount <= 0)
            stackItems.Remove(item);

        UpdateHotbar();
        return true;
    }

    void UpdateHotbar() {
        for (int i = 0; i < slotTexts.Length; i++) {
            slotTexts[i].text = "";
        }

        int slotIndex = 0;
        // items
        for (int i = 0; i < items.Count && slotIndex < slotTexts.Length; i++) {
            slotTexts[slotIndex].text = items[i];
            slotIndex++;
        }
        // stackable items
        for (int i = 0; i < stackItems.Count && slotIndex < slotTexts.Length; i++) {

            slotTexts[slotIndex].text =
                stackItems[i].itemName + "\n" + stackItems[i].amount;

            slotIndex++;
        }
        // key items
        for (int i = 0; i < keys.Count && slotIndex < slotTexts.Length; i++) {
            if (keys[i].uses > 1)
            {
                slotTexts[slotIndex].text = keys[i].displayName + "\n" + keys[i].uses;
            } else {
                slotTexts[slotIndex].text = keys[i].displayName;
            }
            slotIndex++;
        }
    }

    void SelectSlot(int index) {
        if (index >= 0 && index < slotTexts.Length) {
            selectedSlot = index;
            HighlightSelectedSlot();
        }
    }

    void HighlightSelectedSlot() {
        for (int i = 0; i < slotBorders.Length; i++) {
            slotBorders[i].color = (i == selectedSlot) ? Color.grey : Color.white;
        }
    }

    public string GetSelectedItem() {
        if (selectedSlot < items.Count)
            return items[selectedSlot];

        return "";
    }

    public bool HasItem(string itemName)
    {
        return items.Contains(itemName);
    }

    public void AddKey(string keyId, int uses, string displayName)
    {
        KeyData existingKey = keys.Find(k => k.keyId == keyId);

        if (existingKey != null)
        {
            existingKey.uses += uses;
        }
        else
        {
            KeyData newKey = new KeyData();
            newKey.keyId = keyId;
            newKey.displayName = displayName;
            newKey.uses = uses;
            keys.Add(newKey);
        }
        UpdateHotbar();
    }

    public bool HasUsableKey(string keyId)
    {
        KeyData key = keys.Find(k => k.keyId == keyId);
        return key != null && key.uses > 0;
    }

    public int GetKeyUses(string keyId)
    {
        KeyData key = keys.Find(k => k.keyId == keyId);
        return key != null ? key.uses : 0;
    }

    public bool UseKey(string keyId)
    {
        KeyData key = keys.Find(k => k.keyId == keyId);

        if (key == null || key.uses <= 0)
            return false;

        key.uses--;

        if (key.uses <= 0)
        {
            keys.Remove(key);
        }
        UpdateHotbar();
        return true;
    }
}