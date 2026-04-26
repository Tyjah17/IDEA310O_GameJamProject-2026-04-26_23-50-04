using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject gameOverPanel;
    public GameObject winPanel;

    [Header("Player Scripts")]
    public MonoBehaviour playerLookScript;
    public MonoBehaviour playerMoveScript;

    public void ShowGameOver() {
        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        DisablePlayer();
        UnlockCursor();
    }

    public void ShowWin() {
        Time.timeScale = 0f;

        if (winPanel != null)
            winPanel.SetActive(true);

        DisablePlayer();
        UnlockCursor();
    } 

    public void RestartLevel() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void LoadNextLevel() {
        Time.timeScale = 1f;

        int current = SceneManager.GetActiveScene().buildIndex;
        int next = current + 1;

        if (next < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(next);
        else
            SceneManager.LoadScene("Menu");
    }

    void DisablePlayer() {
        if (playerLookScript != null)
            playerLookScript.enabled = false;

        if (playerMoveScript != null)
            playerMoveScript.enabled = false;
    }

    void UnlockCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}