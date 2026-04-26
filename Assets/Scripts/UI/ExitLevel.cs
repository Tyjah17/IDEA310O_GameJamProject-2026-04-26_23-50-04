using UnityEngine;

public class LevelExit : MonoBehaviour {
    public GameUIManager gameUI;

    public void Interact() {
        if (gameUI != null) {
            gameUI.ShowWin();
        }
    }
}