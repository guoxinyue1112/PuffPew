using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private Transform playerTransform;
    private Sprite enemySprite;

    public void Initialize(Transform player, Sprite squareSprite)
    {
        playerTransform = player;
        enemySprite = squareSprite;
    }

    public void SpawnEnemy(EnemyStats stats, WaveManager waveManager)
    {
        Vector2 direction = Random.insideUnitCircle.normalized;
        if (direction == Vector2.zero)
        {
            direction = Vector2.right;
        }

        Vector3 spawnPosition = playerTransform.position + (Vector3)(direction * 10f);
        GameObject enemyObject = new(stats.SizeMultiplier > 1.2f ? "LargeEnemy" : "Enemy");
        enemyObject.transform.position = spawnPosition;

        SpriteRenderer spriteRenderer = enemyObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = enemySprite;
        spriteRenderer.color = enemySprite == PuffPewArt.GetEnemySprite() ? Color.white : new Color(0.95f, 0.38f, 0.38f);
        spriteRenderer.sortingOrder = 10;

        if (enemySprite == PuffPewArt.GetEnemySprite())
        {
            PuffPewArt.SetUniformWorldSize(enemyObject.transform, enemySprite, 1.45f * stats.SizeMultiplier);
        }
        else
        {
            float scale = 0.75f * stats.SizeMultiplier;
            enemyObject.transform.localScale = new Vector3(scale, scale, 1f);
        }

        Rigidbody2D rigidbody2D = enemyObject.AddComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = 0f;
        rigidbody2D.freezeRotation = true;

        CircleCollider2D circleCollider = enemyObject.AddComponent<CircleCollider2D>();
        circleCollider.radius = 0.5f * stats.SizeMultiplier;

        Enemy enemy = enemyObject.AddComponent<Enemy>();
        enemy.Initialize(stats, playerTransform, waveManager);
    }
}
