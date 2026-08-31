using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
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
        return 10 + 2 * (wave - 1);
    }

    public EnemyStats GetScaledEnemyStats()
    {
        float hp = 30f * (1f + 0.10f * (CurrentWave - 1));
        float damage = 10f * (1f + 0.05f * (CurrentWave - 1));
        float speed = 2f * (1f + 0.02f * (CurrentWave - 1));
        return new EnemyStats(Mathf.RoundToInt(hp), damage, speed, 1f, 10);
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
        while (SpawnedThisWave < TotalToSpawnThisWave)
        {
            if (GameManager.Instance.IsTerminalState)
            {
                yield break;
            }

            SpawnedThisWave++;
            enemySpawner.SpawnEnemy(GetScaledEnemyStats(), this);
            yield return new WaitForSeconds(0.6f);
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
}
