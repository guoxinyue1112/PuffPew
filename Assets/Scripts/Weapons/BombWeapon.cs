using UnityEngine;

public class BombWeapon : WeaponBase
{
    private const float ProjectileSpeed = 7f;
    private const float ExplosionRadius = 2f;

    protected override void Awake()
    {
        base.Awake();
        baseDamage = 35f;
        baseAttackInterval = 2.5f;
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
}
