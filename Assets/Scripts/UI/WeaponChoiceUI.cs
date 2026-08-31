using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponChoiceUI : MonoBehaviour
{
    private readonly List<Button> buttons = new();
    private readonly List<TextMeshProUGUI> labels = new();

    private GameManager gameManager;
    private RectTransform panel;

    public void BuildUI()
    {
        panel = UIFactory.CreatePanel(
            transform,
            "WeaponChoicePanel",
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(460f, 260f),
            new Color(0.05f, 0.07f, 0.10f, 0.95f));

        TextMeshProUGUI title = UIFactory.CreateText(panel, "Title", "CHOOSE A WEAPON", 32, TextAlignmentOptions.Center);
        RectTransform titleRect = title.rectTransform;
        titleRect.anchorMin = new Vector2(0f, 1f);
        titleRect.anchorMax = new Vector2(1f, 1f);
        titleRect.pivot = new Vector2(0.5f, 1f);
        titleRect.anchoredPosition = new Vector2(0f, -32f);
        titleRect.sizeDelta = new Vector2(-48f, 48f);

        for (int i = 0; i < 2; i++)
        {
            Button button = UIFactory.CreateButton(panel, $"WeaponButton{i}", string.Empty, null);
            RectTransform rect = button.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);
            rect.anchoredPosition = new Vector2(0f, -98f - i * 84f);
            rect.sizeDelta = new Vector2(360f, 68f);
            buttons.Add(button);
            labels.Add(button.GetComponentInChildren<TextMeshProUGUI>());
        }

        panel.gameObject.SetActive(false);
    }

    public void Initialize(GameManager manager)
    {
        gameManager = manager;
    }

    public void Show()
    {
        List<WeaponType> options = GetDistinctRandomWeapons();
        for (int i = 0; i < buttons.Count; i++)
        {
            WeaponType option = options[i];
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => gameManager.ResolveWeaponChoice(option));
            labels[i].text = option.ToString();
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

    private static List<WeaponType> GetDistinctRandomWeapons()
    {
        List<WeaponType> pool = new()
        {
            WeaponType.Pistol,
            WeaponType.Axe,
            WeaponType.Bomb
        };

        for (int i = 0; i < pool.Count; i++)
        {
            int swapIndex = Random.Range(i, pool.Count);
            (pool[i], pool[swapIndex]) = (pool[swapIndex], pool[i]);
        }

        return pool.GetRange(0, 2);
    }
}
