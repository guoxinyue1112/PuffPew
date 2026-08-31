using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    private readonly List<Button> buttons = new();
    private readonly List<TextMeshProUGUI> buttonLabels = new();

    private GameManager gameManager;
    private RectTransform panel;

    public void BuildUI()
    {
        panel = UIFactory.CreatePanel(
            transform,
            "LevelUpPanel",
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(520f, 360f),
            new Color(0.05f, 0.07f, 0.10f, 0.95f));

        TextMeshProUGUI title = UIFactory.CreateText(panel, "Title", "LEVEL UP", 34, TextAlignmentOptions.Center);
        RectTransform titleRect = title.rectTransform;
        titleRect.anchorMin = new Vector2(0f, 1f);
        titleRect.anchorMax = new Vector2(1f, 1f);
        titleRect.pivot = new Vector2(0.5f, 1f);
        titleRect.anchoredPosition = new Vector2(0f, -34f);
        titleRect.sizeDelta = new Vector2(-48f, 48f);

        for (int i = 0; i < 3; i++)
        {
            Button button = UIFactory.CreateButton(panel, $"UpgradeButton{i}", string.Empty, null);
            RectTransform rect = button.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);
            rect.anchoredPosition = new Vector2(0f, -100f - i * 82f);
            rect.sizeDelta = new Vector2(420f, 64f);
            buttons.Add(button);
            buttonLabels.Add(button.GetComponentInChildren<TextMeshProUGUI>());
        }

        panel.gameObject.SetActive(false);
    }

    public void Initialize(GameManager manager)
    {
        gameManager = manager;
    }

    public void Show(PlayerStats stats)
    {
        List<UpgradeType> options = GetDistinctRandomUpgrades(3);
        for (int i = 0; i < buttons.Count; i++)
        {
            UpgradeType option = options[i];
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => gameManager.ResolveLevelUp(option));
            buttonLabels[i].text = GetUpgradeLabel(option);
        }

        panel.gameObject.SetActive(true);
    }

    public void Hide()
    {
        if (panel != null)
        {
            panel.gameObject.SetActive(false);
        }
    }

    private static List<UpgradeType> GetDistinctRandomUpgrades(int count)
    {
        List<UpgradeType> pool = new()
        {
            UpgradeType.AttackSpeed,
            UpgradeType.AttackDamage,
            UpgradeType.MaxHP,
            UpgradeType.Defense,
            UpgradeType.MoveSpeed
        };

        for (int i = 0; i < pool.Count; i++)
        {
            int swapIndex = UnityEngine.Random.Range(i, pool.Count);
            (pool[i], pool[swapIndex]) = (pool[swapIndex], pool[i]);
        }

        return pool.GetRange(0, count);
    }

    private static string GetUpgradeLabel(UpgradeType upgradeType)
    {
        return upgradeType switch
        {
            UpgradeType.AttackSpeed => "Attack Speed +15%",
            UpgradeType.AttackDamage => "Attack Damage +20%",
            UpgradeType.MaxHP => "Max HP +20",
            UpgradeType.Defense => "Defense +2",
            UpgradeType.MoveSpeed => "Move Speed +10%",
            _ => throw new ArgumentOutOfRangeException(nameof(upgradeType), upgradeType, null)
        };
    }
}
