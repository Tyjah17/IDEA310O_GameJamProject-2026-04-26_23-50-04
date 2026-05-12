using UnityEngine;

public class PowerCell : MonoBehaviour {

    [Header("State")]
    public bool hasPowerCell = false;

    [Header("All Power Cells")]
    public PowerCell[] allPowerCells;

    [Header("Power Restore")]
    public PowerRestore powerRestore;

    [Header("Screens")]
    public GameObject[] screensToTurnOff;
    public GameObject[] screensToTurnOn;

    public string GetInteractText(Hotbar hotbar) {
        if (hasPowerCell)
            return "Power Cell Installed";

        if (hotbar != null && hotbar.GetStackItemAmount("Power Cell") > 0)
            return "Press E to install Power Cell";

        return "Needs Power Cell";
    }

    public void InstallPowerCell(Hotbar hotbar) {
        if (hasPowerCell || hotbar == null)
            return;
        if (!hotbar.UseStackItem("Power Cell", 1))
            return;
        hasPowerCell = true;
        CheckAllPowerCellsInstalled();
    }

    void CheckAllPowerCellsInstalled() {
        foreach (PowerCell cell in allPowerCells) {
            if (cell == null || !cell.hasPowerCell)
                return;
        }
        foreach (GameObject screen in screensToTurnOff) {
            if (screen != null)
                screen.SetActive(false);
        }
        foreach (GameObject screen in screensToTurnOn) {
            if (screen != null)
                screen.SetActive(true);
        }
        if (powerRestore != null) {
            powerRestore.RestorePower();
        }
    }
}