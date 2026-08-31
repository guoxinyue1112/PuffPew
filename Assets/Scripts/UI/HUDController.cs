using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    private PlayerStats playerStats;
    private PlayerExperience playerExperience;
    private WeaponManager weaponManager;
    private WaveManager waveManager;
    private EnemyManager enemyManager;

    private TextMeshProUGUI hudText;

    public void BuildUI()
    {
        RectTransform panel = UIFactory.CreatePanel(
            transform,
            "HUDPanel",
            new Vector2(0f, 1f),
            new Vector2(0f, 1f),
            new Vector2(330f, 220f),
            new Color(0f, 0f, 0f, 0.38f));

        panel.pivot = new Vector2(0f, 1f);
        panel.anchoredPosition = new Vector2(20f, -20f);

        hudText = UIFactory.CreateText(panel, "HUDText", string.Empty, 24, TextAlignmentOptions.TopLeft);
        hudText.margin = new Vector4(16f, 14f, 16f, 14f);
    }

    public void Initialize(PlayerStats stats, PlayerExperience experience, WeaponManager weapons, WaveManager waves, EnemyManager enemies)
    {
        playerStats = stats;
        playerExperience = experience;
        weaponManager = weapons;
        waveManager = waves;
        enemyManager = enemies;
    }

    private void Update()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (hudText == null || playerStats == null)
        {
            return;
        }

        hudText.text =
            $"HP: {playerStats.CurrentHP} / {playerStats.MaxHP}\n" +
            $"Level: {playerExperience.Level}\n" +
            $"XP: {playerExperience.CurrentXP} / {playerExperience.RequiredXP}\n" +
            $"Wave: {Mathf.Max(1, waveManager.CurrentWave)} / {waveManager.TotalWaves}\n" +
            $"Enemies: {enemyManager.AliveCount}\n\n" +
            "Weapons:\n" +
            $"Pistol x{weaponManager.GetWeaponCount(WeaponType.Pistol)}\n" +
            $"Axe x{weaponManager.GetWeaponCount(WeaponType.Axe)}\n" +
            $"Bomb x{weaponManager.GetWeaponCount(WeaponType.Bomb)}";
    }
}
