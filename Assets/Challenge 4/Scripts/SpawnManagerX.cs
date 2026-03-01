using UnityEngine;

public class SpawnManagerX : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject normalPowerupPrefab;
    [SerializeField] private GameObject smashPowerupPrefab;
    [SerializeField] private GameObject freezeEnemyPowerupPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnRangeX = 10f;
    [SerializeField] private float spawnZMin = 15f;
    [SerializeField] private float spawnZMax = 25f;

    [SerializeField] private GameObject player;

    private int waveCount = 1;
    private bool waveActive;
    public int CurrentWave => waveCount;
    private void Update()
    {
        if (!UIManager.Instance.isGameStarted) return;
        if (!waveActive && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        waveActive = true;

        SpawnPowerups();
        // Base enemy count on wave number
        int enemyCount = waveCount;

        if (GameSettings.ExtraEnemyPerWave)
            enemyCount += 1;

        SpawnEnemies(enemyCount);

        waveCount++;
        ResetPlayerPosition();
        if (waveCount > 1)
        {
            UIManager.Instance.ShowNewWaveUI();
        }
        waveActive = false;
    }

    private void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, GenerateSpawnPosition(), Quaternion.identity);

            EnemyX enemyScript = enemy.GetComponent<EnemyX>();

            // Random AI type
            enemyScript.enemyType = (EnemyType)Random.Range(0, 3);
        }
    }

    private void SpawnPowerups()
    {
        Vector3 offset = new Vector3(0, 0, -15);

        if (GameObject.FindGameObjectsWithTag("Powerup").Length == 0)
            Instantiate(normalPowerupPrefab, GenerateSpawnPosition() + offset, Quaternion.identity);

        if (GameObject.FindGameObjectsWithTag("SmashPowerup").Length == 0)
            Instantiate(smashPowerupPrefab, GenerateSpawnPosition() + offset, Quaternion.identity);

        if (GameObject.FindGameObjectsWithTag("FreezePowerup").Length == 0)
            Instantiate(freezeEnemyPowerupPrefab, GenerateSpawnPosition() + offset, Quaternion.identity);
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