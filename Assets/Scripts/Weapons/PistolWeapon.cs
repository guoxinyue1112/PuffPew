using UnityEngine;

public class PistolWeapon : WeaponBase
{
    private const float ProjectileSpeed = 12f;
    private const float ProjectileLifetime = 3f;

    protected override void Awake()
    {
        base.Awake();
        baseDamage = 10f;
        baseAttackInterval = 0.7f;
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
}
