using UnityEngine;
using UnityEngine.UI;

public enum HeartStatus {
    Empty = 0,
    Full = 1
}

public class Hearts : MonoBehaviour {
    public Sprite fullHeart, emptyHeart;
    Image heartImage;

    private void Awake() {
        heartImage = GetComponent<Image>();
    }

    public void SetHeartImage(HeartStatus status) {
        switch (status) {
            case HeartStatus.Empty:
                heartImage.sprite = emptyHeart;
                break;
            case HeartStatus.Full:
                heartImage.sprite = fullHeart;
                break;
        }
    }
}