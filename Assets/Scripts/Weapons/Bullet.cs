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

        SpriteRenderer spriteRenderer = bulletObject.AddComponent<SpriteRenderer>();
        Sprite bulletSprite = PuffPewArt.GetBulletSprite();
        spriteRenderer.sprite = bulletSprite != null ? bulletSprite : RuntimeSpriteFactory.GetCircleSprite();
        spriteRenderer.color = bulletSprite != null ? Color.white : new Color(0.95f, 0.95f, 0.95f);
        spriteRenderer.sortingOrder = 20;

        if (bulletSprite != null)
        {
            PuffPewArt.SetUniformWorldSize(bulletObject.transform, bulletSprite, 0.55f);
        }
        else
        {
            bulletObject.transform.localScale = new Vector3(0.25f, 0.25f, 1f);
        }

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

        float angle = Mathf.Atan2(bullet.direction.y, bullet.direction.x) * Mathf.Rad2Deg;
        bulletObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);
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
