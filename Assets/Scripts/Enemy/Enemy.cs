using UnityEngine;

public readonly struct EnemyStats
{
    public EnemyStats(int hp, float damage, float moveSpeed, float contactInterval, int xpReward)
    {
        HP = hp;
        Damage = damage;
        MoveSpeed = moveSpeed;
        ContactInterval = contactInterval;
        XPReward = xpReward;
    }

    public int HP { get; }
    public float Damage { get; }
    public float MoveSpeed { get; }
    public float ContactInterval { get; }
    public int XPReward { get; }
}

public class Enemy : MonoBehaviour
{
    public int CurrentHP { get; private set; }

    private Transform playerTransform;
    private WaveManager waveManager;
    private Rigidbody2D rb;
    private float damage;
    private float moveSpeed;
    private float contactInterval;
    private int xpReward;
    private float contactCooldown;
    private bool dead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(EnemyStats stats, Transform target, WaveManager ownerWaveManager)
    {
        CurrentHP = stats.HP;
        damage = stats.Damage;
        moveSpeed = stats.MoveSpeed;
        contactInterval = stats.ContactInterval;
        xpReward = stats.XPReward;
        playerTransform = target;
        waveManager = ownerWaveManager;
        GameManager.Instance.EnemyManager.Register(this);
        waveManager.NotifyEnemySpawned();
    }

    private void Update()
    {
        if (dead || !GameManager.Instance.GameplayRunning || playerTransform == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        if (contactCooldown > 0f)
        {
            contactCooldown -= Time.deltaTime;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (dead || contactCooldown > 0f || !collision.collider.CompareTag("Player"))
        {
            return;
        }

        PlayerStats playerStats = collision.collider.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.TakeDamage(damage);
            contactCooldown = contactInterval;
        }
    }

    public void TakeDamage(float amount)
    {
        if (dead)
        {
            return;
        }

        int actualDamage = Mathf.Max(1, Mathf.RoundToInt(amount));
        CurrentHP -= actualDamage;
        GameManager.Instance.SpawnFloatingText(actualDamage.ToString(), transform.position + Vector3.up * 0.9f, new Color(1f, 0.92f, 0.4f));

        if (CurrentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (dead)
        {
            return;
        }

        dead = true;
        GameManager.Instance.EnemyManager.Unregister(this);
        waveManager.NotifyEnemyKilled();
        XPOrb.Create(transform.position, xpReward);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EnemyManager?.Unregister(this);
        }
    }
}
