using UnityEngine;

public class PistolWeapon : WeaponBase
{
    private const float ProjectileSpeed = 12f;
    private const float ProjectileLifetime = 3f;

    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        baseDamage = 10f;
        baseAttackInterval = 0.7f;
        SetupVisual();
    }

    protected override void Update()
    {
        AimAtNearestEnemy();
        base.Update();
    }

    protected override bool TryAttack()
    {
        Enemy target = GetNearestEnemy();
        if (target == null)
        {
            return false;
        }

        Vector2 direction = (target.transform.position - transform.position).normalized;
        Bullet.Create(transform.position, direction, GetDamage(), ProjectileSpeed, ProjectileLifetime);
        return true;
    }

    private void SetupVisual()
    {
        Sprite pistolSprite = PuffPewArt.GetPistolSprite();
        if (pistolSprite == null)
        {
            return;
        }

        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = pistolSprite;
        spriteRenderer.color = Color.white;
        spriteRenderer.sortingOrder = 5;
        PuffPewArt.SetUniformWorldSize(transform, pistolSprite, 0.95f);
    }

    private void AimAtNearestEnemy()
    {
        if (spriteRenderer == null)
        {
            return;
        }

        Enemy target = GetNearestEnemy();
        if (target == null)
        {
            return;
        }

        Vector2 direction = (target.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
