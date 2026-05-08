using UnityEngine;

public class KeyRandomSpawner : MonoBehaviour
{
    [Header("Key Object")]
    public GameObject keyPrefab;

    [Header("Normal Spawn Points")]
    public Transform[] normalSpawnPoints;

    [Header("Rare Near-Door Spawn")]
    public Transform nearDoorSpawnPoint;
    [Range(0f, 1f)]
    public float nearDoorChance = 0.01f;

    private void Start()
    {
        SpawnKey();
    }

    private void SpawnKey()
    {
        if (keyPrefab == null)
        {
            return;
        }

        Transform chosenPoint = null;
        float roll = Random.value;

        if (nearDoorSpawnPoint != null && roll <= nearDoorChance)
        {
            chosenPoint = nearDoorSpawnPoint;
        }
        else
        {
            if (normalSpawnPoints == null || normalSpawnPoints.Length == 0)
            {
                return;
            }

            int randomIndex = Random.Range(0, normalSpawnPoints.Length);
            chosenPoint = normalSpawnPoints[randomIndex];
        }

        GameObject spawnedKey = Instantiate(keyPrefab, chosenPoint.position, chosenPoint.rotation);

        if (spawnedKey != null)
        {
            Debug.Log("Spawned key: " + spawnedKey.name + " at " + chosenPoint.position);
        }
        else
        {
            Debug.LogError("Instantiate failed.");
        }
    }
}