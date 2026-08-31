using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    private struct WeaponSlotRefs
    {
        public Image Icon;
        public TextMeshProUGUI CountText;
    }

    private PlayerStats playerStats;
    private PlayerExperience playerExperience;
    private WeaponManager weaponManager;
    private WaveManager waveManager;
    private EnemyManager enemyManager;

    private TextMeshProUGUI levelText;
    private TextMeshProUGUI waveInfoText;
    private UIFactory.ProgressBarRefs hpBar;
    private UIFactory.ProgressBarRefs xpBar;
    private WeaponSlotRefs pistolSlot;
    private WeaponSlotRefs axeSlot;
    private WeaponSlotRefs bombSlot;

    public void BuildUI()
    {
        ClearExistingUI();

        RectTransform topLeftPanel = UIFactory.CreatePanel(
            transform,
            "TopLeftHUD",
            new Vector2(0f, 1f),
            new Vector2(0f, 1f),
            new Vector2(380f, 120f),
            new Color(0.02f, 0.03f, 0.05f, 0.55f));
        topLeftPanel.pivot = new Vector2(0f, 1f);
        topLeftPanel.anchoredPosition = new Vector2(20f, -20f);

        hpBar = UIFactory.CreateProgressBar(topLeftPanel, "HPBar", new Color(0.16f, 0.08f, 0.08f, 0.95f), new Color(0.88f, 0.22f, 0.22f, 1f));
        RectTransform hpRect = hpBar.Fill.transform.parent.parent.GetComponent<RectTransform>();
        hpRect.anchorMin = new Vector2(0f, 1f);
        hpRect.anchorMax = new Vector2(1f, 1f);
        hpRect.pivot = new Vector2(0f, 1f);
        hpRect.anchoredPosition = new Vector2(16f, -16f);
        hpRect.sizeDelta = new Vector2(-32f, 28f);

        xpBar = UIFactory.CreateProgressBar(topLeftPanel, "XPBar", new Color(0.08f, 0.12f, 0.18f, 0.95f), new Color(0.20f, 0.62f, 1f, 1f));
        RectTransform xpRect = xpBar.Fill.transform.parent.parent.GetComponent<RectTransform>();
        xpRect.anchorMin = new Vector2(0f, 1f);
        xpRect.anchorMax = new Vector2(1f, 1f);
        xpRect.pivot = new Vector2(0f, 1f);
        xpRect.anchoredPosition = new Vector2(16f, -70f);
        xpRect.sizeDelta = new Vector2(-108f, 28f);

        levelText = UIFactory.CreateText(topLeftPanel, "LevelText", string.Empty, 22, TextAlignmentOptions.Center);
        RectTransform levelRect = levelText.rectTransform;
        levelRect.anchorMin = new Vector2(1f, 1f);
        levelRect.anchorMax = new Vector2(1f, 1f);
        levelRect.pivot = new Vector2(1f, 1f);
        levelRect.anchoredPosition = new Vector2(-26f, -70f);
        levelRect.sizeDelta = new Vector2(76f, 28f);

        RectTransform topRightPanel = UIFactory.CreatePanel(
            transform,
            "TopRightHUD",
            new Vector2(1f, 1f),
            new Vector2(1f, 1f),
            new Vector2(220f, 96f),
            new Color(0.02f, 0.03f, 0.05f, 0.55f));
        topRightPanel.pivot = new Vector2(1f, 1f);
        topRightPanel.anchoredPosition = new Vector2(-20f, -20f);

        waveInfoText = UIFactory.CreateText(topRightPanel, "WaveInfoText", string.Empty, 22, TextAlignmentOptions.TopRight);
        waveInfoText.margin = new Vector4(12f, 12f, 16f, 12f);
        RectTransform waveRect = waveInfoText.rectTransform;
        waveRect.anchorMin = Vector2.zero;
        waveRect.anchorMax = Vector2.one;
        waveRect.offsetMin = Vector2.zero;
        waveRect.offsetMax = Vector2.zero;

        RectTransform bottomLeftPanel = UIFactory.CreatePanel(
            transform,
            "BottomLeftHUD",
            new Vector2(0f, 0f),
            new Vector2(0f, 0f),
            new Vector2(260f, 168f),
            new Color(0.02f, 0.03f, 0.05f, 0.55f));
        bottomLeftPanel.pivot = new Vector2(0f, 0f);
        bottomLeftPanel.anchoredPosition = new Vector2(20f, 20f);

        CreateWeaponPanel(bottomLeftPanel);
    }

    public void ApplyEditorPreview()
    {
        if (levelText == null)
        {
            return;
        }

        hpBar.FillRect.anchorMax = new Vector2(0.62f, 1f);
        hpBar.Label.text = "HP  62 / 100";

        xpBar.FillRect.anchorMax = new Vector2(0.38f, 1f);
        xpBar.Label.text = "XP  30 / 80";

        levelText.text = "Lv.3";
        waveInfoText.text = "Wave: 2 / 5\nEnemies: 14";

        RefreshWeaponSlot(pistolSlot, PuffPewArt.GetPistolSprite(), 2, "Pistol");
        RefreshWeaponSlot(axeSlot, PuffPewArt.GetAxeSprite(), 1, "Axe");
        RefreshWeaponSlot(bombSlot, PuffPewArt.GetBombSprite(), 1, "Bomb");
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
        if (levelText == null || playerStats == null)
        {
            return;
        }

        float hpPercent = playerStats.MaxHP > 0 ? Mathf.Clamp01((float)playerStats.CurrentHP / playerStats.MaxHP) : 0f;
        float xpPercent = playerExperience.RequiredXP > 0 ? Mathf.Clamp01((float)playerExperience.CurrentXP / playerExperience.RequiredXP) : 0f;

        hpBar.FillRect.anchorMax = new Vector2(hpPercent, 1f);
        hpBar.Label.text = $"HP  {playerStats.CurrentHP} / {playerStats.MaxHP}";

        xpBar.FillRect.anchorMax = new Vector2(xpPercent, 1f);
        xpBar.Label.text = $"XP  {playerExperience.CurrentXP} / {playerExperience.RequiredXP}";

        levelText.text = $"Lv.{playerExperience.Level}";
        waveInfoText.text =
            $"Wave: {Mathf.Max(1, waveManager.CurrentWave)} / {waveManager.TotalWaves}\n" +
            $"Enemies: {enemyManager.AliveCount}";

        RefreshWeaponSlot(pistolSlot, PuffPewArt.GetPistolSprite(), weaponManager.GetWeaponCount(WeaponType.Pistol), "Pistol");
        RefreshWeaponSlot(axeSlot, PuffPewArt.GetAxeSprite(), weaponManager.GetWeaponCount(WeaponType.Axe), "Axe");
        RefreshWeaponSlot(bombSlot, PuffPewArt.GetBombSprite(), weaponManager.GetWeaponCount(WeaponType.Bomb), "Bomb");
    }

    private void CreateWeaponPanel(RectTransform parent)
    {
        pistolSlot = CreateWeaponSlot(parent, "PistolSlot", new Vector2(84f, -24f));
        axeSlot = CreateWeaponSlot(parent, "AxeSlot", new Vector2(84f, -66f));
        bombSlot = CreateWeaponSlot(parent, "BombSlot", new Vector2(84f, -108f));
    }

    private WeaponSlotRefs CreateWeaponSlot(RectTransform parent, string name, Vector2 anchoredPosition)
    {
        GameObject slotObject = new(name);
        slotObject.transform.SetParent(parent, false);

        RectTransform slotRect = slotObject.AddComponent<RectTransform>();
        slotRect.anchorMin = new Vector2(0f, 1f);
        slotRect.anchorMax = new Vector2(1f, 1f);
        slotRect.pivot = new Vector2(0f, 1f);
        slotRect.anchoredPosition = anchoredPosition;
        slotRect.sizeDelta = new Vector2(-32f, 36f);

        GameObject iconObject = new("Icon");
        iconObject.transform.SetParent(slotObject.transform, false);
        Image icon = iconObject.AddComponent<Image>();
        icon.preserveAspect = true;

        RectTransform iconRect = icon.rectTransform;
        iconRect.anchorMin = new Vector2(0f, 0.5f);
        iconRect.anchorMax = new Vector2(0f, 0.5f);
        iconRect.pivot = new Vector2(0f, 0.5f);
        iconRect.anchoredPosition = new Vector2(0f, 0f);
        iconRect.sizeDelta = new Vector2(32f, 32f);

        TextMeshProUGUI countText = UIFactory.CreateText(slotObject.transform, "Count", string.Empty, 20, TextAlignmentOptions.MidlineLeft);
        RectTransform countRect = countText.rectTransform;
        countRect.anchorMin = new Vector2(0f, 0f);
        countRect.anchorMax = new Vector2(1f, 1f);
        countRect.offsetMin = new Vector2(44f, 0f);
        countRect.offsetMax = Vector2.zero;

        return new WeaponSlotRefs
        {
            Icon = icon,
            CountText = countText
        };
    }

    private static void RefreshWeaponSlot(WeaponSlotRefs slot, Sprite iconSprite, int count, string fallbackName)
    {
        slot.Icon.sprite = iconSprite;
        slot.Icon.enabled = iconSprite != null;
        slot.CountText.text = $"{fallbackName} x{count}";
    }

    private void ClearExistingUI()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (Application.isPlaying)
            {
                Destroy(child);
            }
            else
            {
                DestroyImmediate(child);
            }
        }
    }
}
