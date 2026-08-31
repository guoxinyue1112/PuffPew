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
        GameObject enemyObject = new("Enemy");
        enemyObject.transform.position = spawnPosition;
        enemyObject.transform.localScale = new Vector3(0.75f, 0.75f, 1f);

        SpriteRenderer spriteRenderer = enemyObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = enemySprite;
        spriteRenderer.color = new Color(0.95f, 0.38f, 0.38f);

        Rigidbody2D rigidbody2D = enemyObject.AddComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = 0f;
        rigidbody2D.freezeRotation = true;

        CircleCollider2D circleCollider = enemyObject.AddComponent<CircleCollider2D>();
        circleCollider.radius = 0.5f;

        Enemy enemy = enemyObject.AddComponent<Enemy>();
        enemy.Initialize(stats, playerTransform, waveManager);
    }
}
