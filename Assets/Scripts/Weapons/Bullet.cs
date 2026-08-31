using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 direction;
    private float damage;
    private float speed;
    private float lifetime;

    public static void Create(Vector3 origin, Vector2 travelDirection, float projectileDamage, float projectileSpeed, float projectileLifetime)
    {
        GameObject bulletObject = new("Bullet");
        bulletObject.transform.position = origin;
        bulletObject.transform.localScale = new Vector3(0.25f, 0.25f, 1f);

        SpriteRenderer spriteRenderer = bulletObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = RuntimeSpriteFactory.GetCircleSprite();
        spriteRenderer.color = new Color(0.95f, 0.95f, 0.95f);

        CircleCollider2D collider2D = bulletObject.AddComponent<CircleCollider2D>();
        collider2D.isTrigger = true;

        Rigidbody2D rigidbody2D = bulletObject.AddComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = 0f;
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;

        Bullet bullet = bulletObject.AddComponent<Bullet>();
        bullet.direction = travelDirection.normalized;
        bullet.damage = projectileDamage;
        bullet.speed = projectileSpeed;
        bullet.lifetime = projectileLifetime;
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy == null)
        {
            return;
        }

        enemy.TakeDamage(damage);
        Destroy(gameObject);
    }
}
