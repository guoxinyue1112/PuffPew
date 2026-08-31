using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private const int LargeEnemyStartWave = 5;

    public int CurrentWave { get; private set; }
    public int TotalWaves => 20;
    public int SpawnedThisWave { get; private set; }
    public int TotalToSpawnThisWave { get; private set; }

    private EnemyManager enemyManager;
    private EnemySpawner enemySpawner;
    private Coroutine spawnRoutine;
    private int aliveThisWave;

    public void Initialize(EnemyManager manager, EnemySpawner spawner)
    {
        enemyManager = manager;
        enemySpawner = spawner;
    }

    public void BeginFirstWave()
    {
        BeginWave(1);
    }

    public void BeginNextWave()
    {
        if (CurrentWave >= TotalWaves || GameManager.Instance.IsTerminalState)
        {
            return;
        }

        BeginWave(CurrentWave + 1);
    }

    public int GetEnemyCountForWave(int wave)
    {
        return 12 + 3 * (wave - 1) + GetLargeEnemyCountForWave(wave);
    }

    public EnemyStats GetScaledEnemyStats()
    {
        float hp = 32f * (1f + 0.16f * (CurrentWave - 1));
        float damage = 10f * (1f + 0.08f * (CurrentWave - 1));
        float speed = 2f * (1f + 0.02f * (CurrentWave - 1));
        return new EnemyStats(Mathf.RoundToInt(hp), damage, speed, 1f, 10, 1f);
    }

    public EnemyStats GetScaledLargeEnemyStats()
    {
        float hp = 320f * (1f + 0.22f * (CurrentWave - 1));
        float damage = 18f * (1f + 0.10f * (CurrentWave - 1));
        float speed = 1.25f * (1f + 0.015f * (CurrentWave - 1));
        return new EnemyStats(Mathf.RoundToInt(hp), damage, speed, 0.9f, 28, 3f);
    }

    public void NotifyEnemySpawned()
    {
        aliveThisWave++;
    }

    public void NotifyEnemyKilled()
    {
        aliveThisWave = Mathf.Max(0, aliveThisWave - 1);
        CheckWaveCompletion();
    }

    private void BeginWave(int wave)
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
        }

        CurrentWave = wave;
        SpawnedThisWave = 0;
        aliveThisWave = 0;
        TotalToSpawnThisWave = GetEnemyCountForWave(wave);
        spawnRoutine = StartCoroutine(SpawnWaveRoutine());
    }

    private IEnumerator SpawnWaveRoutine()
    {
        int largeEnemyCount = GetLargeEnemyCountForWave(CurrentWave);
        int largeEnemiesSpawned = 0;

        while (SpawnedThisWave < TotalToSpawnThisWave)
        {
            if (GameManager.Instance.IsTerminalState)
            {
                yield break;
            }

            SpawnedThisWave++;
            bool shouldSpawnLargeEnemy =
                largeEnemiesSpawned < largeEnemyCount &&
                CurrentWave >= LargeEnemyStartWave &&
                (SpawnedThisWave % 5 == 0 || TotalToSpawnThisWave - SpawnedThisWave < largeEnemyCount - largeEnemiesSpawned);

            if (shouldSpawnLargeEnemy)
            {
                enemySpawner.SpawnEnemy(GetScaledLargeEnemyStats(), this);
                largeEnemiesSpawned++;
            }
            else
            {
                enemySpawner.SpawnEnemy(GetScaledEnemyStats(), this);
            }

            yield return new WaitForSeconds(GetSpawnIntervalForWave(CurrentWave));
        }

        spawnRoutine = null;
        CheckWaveCompletion();
    }

    private void CheckWaveCompletion()
    {
        if (GameManager.Instance.IsTerminalState)
        {
            return;
        }

        if (SpawnedThisWave >= TotalToSpawnThisWave && aliveThisWave <= 0)
        {
            GameManager.Instance.NotifyWaveCompleted(CurrentWave);
        }
    }

    private int GetLargeEnemyCountForWave(int wave)
    {
        if (wave < LargeEnemyStartWave)
        {
            return 0;
        }

        return 1 + ((wave - LargeEnemyStartWave) / 2);
    }

    private float GetSpawnIntervalForWave(int wave)
    {
        return Mathf.Max(0.28f, 0.52f - (wave - 1) * 0.015f);
    }
}
