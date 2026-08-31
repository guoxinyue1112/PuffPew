using UnityEngine;

public enum UpgradeType
{
    AttackSpeed,
    AttackDamage,
    MaxHP,
    Defense,
    MoveSpeed
}

public class PlayerStats : MonoBehaviour
{
    public int MaxHP { get; private set; } = 100;
    public int CurrentHP { get; private set; } = 100;
    public float AttackMultiplier { get; private set; } = 1f;
    public float AttackSpeedMultiplier { get; private set; } = 1f;
    public int Defense { get; private set; }
    public float MoveSpeed { get; private set; } = 5f;

    public void TakeDamage(float incomingDamage)
    {
        if (GameManager.Instance.IsTerminalState)
        {
            return;
        }

        int actualDamage = Mathf.Max(1, Mathf.RoundToInt(incomingDamage) - Defense);
        CurrentHP = Mathf.Max(0, CurrentHP - actualDamage);
        GameManager.Instance.SpawnFloatingText(actualDamage.ToString(), transform.position + Vector3.up * 1.2f, new Color(1f, 0.45f, 0.45f));

        if (CurrentHP <= 0)
        {
            GameManager.Instance.NotifyPlayerDied();
        }
    }

    public void ApplyUpgrade(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.AttackSpeed:
                AttackSpeedMultiplier *= 1.15f;
                break;
            case UpgradeType.AttackDamage:
                AttackMultiplier *= 1.20f;
                break;
            case UpgradeType.MaxHP:
                MaxHP += 20;
                CurrentHP = Mathf.Min(MaxHP, CurrentHP + 20);
                break;
            case UpgradeType.Defense:
                Defense += 2;
                break;
            case UpgradeType.MoveSpeed:
                MoveSpeed *= 1.10f;
                break;
        }
    }

    public float GetScaledDamage(float baseDamage)
    {
        return baseDamage * AttackMultiplier;
    }

    public float GetScaledInterval(float baseInterval)
    {
        return baseInterval / AttackSpeedMultiplier;
    }
}
