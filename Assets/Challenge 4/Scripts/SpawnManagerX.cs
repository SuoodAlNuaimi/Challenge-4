using UnityEngine;
using System.Collections.Generic;

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
    [SerializeField] private Vector3 powerupOffset = new Vector3(0, 0, -15);

    [Header("References")]
    [SerializeField] private GameObject player;

    private int waveCount = 1;
    private bool waveActive;

    public int CurrentWave => waveCount;

    private void Update()
    {
        if (!UIManager.Instance.isGameStarted || waveActive) return;

        if (IsWaveCleared())
            SpawnWave();
    }

    private bool IsWaveCleared()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length == 0;
    }

    private void SpawnWave()
    {
        waveActive = true;

        SpawnAllPowerups();

        int enemyCount = waveCount;
        if (GameSettings.ExtraEnemyPerWave)
            enemyCount++;

        SpawnEnemies(enemyCount);

        waveCount++;

        ResetPlayer();

        if (waveCount > 1)
            UIManager.Instance.ShowNewWaveUI();

        waveActive = false;
    }

    private void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, GetSpawnPosition(), Quaternion.identity);
            enemy.GetComponent<EnemyX>().enemyType =
                (EnemyType)Random.Range(0, System.Enum.GetValues(typeof(EnemyType)).Length);
        }
    }

    private void SpawnAllPowerups()
    {
        TrySpawnPowerup("Powerup", normalPowerupPrefab);
        TrySpawnPowerup("SmashPowerup", smashPowerupPrefab);
        TrySpawnPowerup("FreezePowerup", freezeEnemyPowerupPrefab);
    }

    private void TrySpawnPowerup(string tag, GameObject prefab)
    {
        if (GameObject.FindGameObjectsWithTag(tag).Length == 0)
            Instantiate(prefab, GetSpawnPosition() + powerupOffset, Quaternion.identity);
    }

    private Vector3 GetSpawnPosition()
    {
        float x = Random.Range(-spawnRangeX, spawnRangeX);
        float z = Random.Range(spawnZMin, spawnZMax);
        return new Vector3(x, 0f, z);
    }

    private void ResetPlayer()
    {
        player.transform.position = new Vector3(0f, 1f, -7f);

        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}