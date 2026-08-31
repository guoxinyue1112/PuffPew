using UnityEngine;

public class BombWeapon : WeaponBase
{
    private const float ProjectileSpeed = 7f;
    private const float ExplosionRadius = 2f;

    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        baseDamage = 35f;
        baseAttackInterval = 2.5f;
        SetupVisual();
    }

    protected override void Update()
    {
        SpinIdleVisual();
        base.Update();
    }

    protected override bool TryAttack()
    {
        Enemy target = GetNearestEnemy();
        if (target == null)
        {
            return false;
        }

        BombProjectile.Create(transform.position, target.transform.position, GetDamage(), ProjectileSpeed, ExplosionRadius);
        return true;
    }

    private void SetupVisual()
    {
        Sprite bombSprite = PuffPewArt.GetBombSprite();
        if (bombSprite == null)
        {
            return;
        }

        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = bombSprite;
        spriteRenderer.color = Color.white;
        spriteRenderer.sortingOrder = 5;
        PuffPewArt.SetUniformWorldSize(transform, bombSprite, 0.8f);
    }

    private void SpinIdleVisual()
    {
        if (spriteRenderer != null)
        {
            transform.Rotate(0f, 0f, 45f * Time.deltaTime);
        }
    }
}
