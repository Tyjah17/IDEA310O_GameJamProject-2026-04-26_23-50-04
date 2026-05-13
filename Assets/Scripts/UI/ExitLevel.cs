using UnityEngine;

public class LevelExit : MonoBehaviour {

    [Header("UI")]
    public GameUIManager gameUI;

    [Header("Required Key")]
    public string requiredKeyId = "SpaceshipKey";

    public void Interact() {
        Hotbar hotbar = FindObjectOfType<Hotbar>();
        if (hotbar == null)
            return;

        foreach (Hotbar.KeyData key in hotbar.keys) {
            if (key.keyId == requiredKeyId) {
                if (gameUI != null) {
                    gameUI.ShowWin();
                }
                return;
            }
        }
    }
}