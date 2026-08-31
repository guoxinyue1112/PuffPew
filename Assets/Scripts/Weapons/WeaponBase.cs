using UnityEngine;

public enum WeaponType
{
    Pistol,
    Axe,
    Bomb
}

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] protected float baseDamage;
    [SerializeField] protected float baseAttackInterval;

    protected PlayerStats ownerStats;
    private float cooldown;

    protected virtual void Awake()
    {
        ownerStats = GetComponentInParent<PlayerStats>();
    }

    protected virtual void Update()
    {
        if (!GameManager.Instance.GameplayRunning)
        {
            return;
        }

        cooldown -= Time.deltaTime;
        if (cooldown > 0f)
        {
            return;
        }

        if (TryAttack())
        {
            cooldown = ownerStats.GetScaledInterval(baseAttackInterval);
        }
    }

    protected Enemy GetNearestEnemy()
    {
        return GameManager.Instance.EnemyManager.GetNearestEnemy(transform.position);
    }

    protected float GetDamage()
    {
        return ownerStats.GetScaledDamage(baseDamage);
    }

    protected abstract bool TryAttack();
}
