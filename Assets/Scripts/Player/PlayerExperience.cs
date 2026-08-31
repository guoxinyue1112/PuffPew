using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    public int Level { get; private set; } = 1;
    public int CurrentXP { get; private set; }
    public int RequiredXP { get; private set; } = 50;
    public int PendingLevelUps { get; private set; }

    public void AddExperience(int amount)
    {
        if (GameManager.Instance.IsTerminalState)
        {
            return;
        }

        CurrentXP += amount;
        while (CurrentXP >= RequiredXP)
        {
            CurrentXP -= RequiredXP;
            Level++;
            RequiredXP = Mathf.CeilToInt((RequiredXP * 1.25f) / 10f) * 10;
            PendingLevelUps++;
        }

        if (PendingLevelUps > 0)
        {
            GameManager.Instance.NotifyLevelUpAvailable();
        }
    }

    public void ConsumePendingLevelUp()
    {
        PendingLevelUps = Mathf.Max(0, PendingLevelUps - 1);
    }
}
