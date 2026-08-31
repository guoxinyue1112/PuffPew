using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private readonly List<Enemy> activeEnemies = new();

    public IReadOnlyList<Enemy> ActiveEnemies => activeEnemies;
    public int AliveCount => activeEnemies.Count;

    public void Register(Enemy enemy)
    {
        if (enemy != null && !activeEnemies.Contains(enemy))
        {
            activeEnemies.Add(enemy);
        }
    }

    public void Unregister(Enemy enemy)
    {
        if (enemy != null)
        {
            activeEnemies.Remove(enemy);
        }
    }

    public Enemy GetNearestEnemy(Vector3 origin)
    {
        Enemy nearest = null;
        float bestDistanceSqr = float.MaxValue;

        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            Enemy enemy = activeEnemies[i];
            if (enemy == null)
            {
                activeEnemies.RemoveAt(i);
                continue;
            }

            float distanceSqr = (enemy.transform.position - origin).sqrMagnitude;
            if (distanceSqr < bestDistanceSqr)
            {
                bestDistanceSqr = distanceSqr;
                nearest = enemy;
            }
        }

        return nearest;
    }
}
