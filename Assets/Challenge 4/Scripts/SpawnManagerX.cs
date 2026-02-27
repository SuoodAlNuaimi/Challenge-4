using UnityEngine;

public class SpawnManagerX : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject normalPowerupPrefab;
    [SerializeField] private GameObject smashPowerupPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnRangeX = 10f;
    [SerializeField] private float spawnZMin = 15f;
    [SerializeField] private float spawnZMax = 25f;

    [SerializeField] private GameObject player;

    private int waveCount = 1;
    private bool waveActive;

    private void Update()
    {
        if (!waveActive && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        waveActive = true;

        SpawnPowerups();
        SpawnEnemies(waveCount);

        waveCount++;
        ResetPlayerPosition();

        waveActive = false;
    }

    private void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), Quaternion.identity);
        }
    }

    private void SpawnPowerups()
    {
        Vector3 offset = new Vector3(0, 0, -15);

        if (GameObject.FindGameObjectsWithTag("Powerup").Length == 0)
        {
            Instantiate(normalPowerupPrefab, GenerateSpawnPosition() + offset, Quaternion.identity);
        }

        if (GameObject.FindGameObjectsWithTag("SmashPowerup").Length == 0)
        {
            Instantiate(smashPowerupPrefab, GenerateSpawnPosition() + offset, Quaternion.identity);
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float x = Random.Range(-spawnRangeX, spawnRangeX);
        float z = Random.Range(spawnZMin, spawnZMax);
        return new Vector3(x, 0, z);
    }

    private void ResetPlayerPosition()
    {
        player.transform.position = new Vector3(0, 1, -7);

        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}