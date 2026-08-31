using UnityEngine;

public class BombProjectile : MonoBehaviour
{
    private const float RotationSpeed = 200f;

    private Vector3 targetPosition;
    private float damage;
    private float speed;
    private float explosionRadius;
    private bool hasExploded;

    public static void Create(Vector3 origin, Vector3 lockedTargetPosition, float projectileDamage, float projectileSpeed, float radius)
    {
        GameObject projectileObject = new("BombProjectile");
        projectileObject.transform.position = origin;

        SpriteRenderer spriteRenderer = projectileObject.AddComponent<SpriteRenderer>();
        Sprite bombSprite = PuffPewArt.GetBombSprite();
        spriteRenderer.sprite = bombSprite != null ? bombSprite : RuntimeSpriteFactory.GetCircleSprite();
        spriteRenderer.color = bombSprite != null ? Color.white : new Color(1f, 0.65f, 0.25f);
        spriteRenderer.sortingOrder = 20;

        if (bombSprite != null)
        {
            PuffPewArt.SetUniformWorldSize(projectileObject.transform, bombSprite, 0.9f);
        }
        else
        {
            projectileObject.transform.localScale = new Vector3(0.4f, 0.4f, 1f);
        }

        CircleCollider2D collider2D = projectileObject.AddComponent<CircleCollider2D>();
        collider2D.isTrigger = true;
        collider2D.radius = 0.45f;

        Rigidbody2D rigidbody2D = projectileObject.AddComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = 0f;
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;

        BombProjectile projectile = projectileObject.AddComponent<BombProjectile>();
        projectile.targetPosition = lockedTargetPosition;
        projectile.damage = projectileDamage;
        projectile.speed = projectileSpeed;
        projectile.explosionRadius = radius;
    }

    private void Update()
    {
        if (hasExploded)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        transform.Rotate(0f, 0f, RotationSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) <= 0.02f)
        {
            Explode();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasExploded || other.GetComponent<Enemy>() == null)
        {
            return;
        }

        Explode();
    }

    private void Explode()
    {
        if (hasExploded)
        {
            return;
        }

        hasExploded = true;
        BombExplosionEffect.Create(transform.position, explosionRadius);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        GameManager.Instance.SpawnFloatingText("BOOM", transform.position + Vector3.up * 0.25f, new Color(1f, 0.6f, 0.2f));
        Destroy(gameObject);
    }
}
