using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class UIFactory
{
    public struct ProgressBarRefs
    {
        public Image Fill;
        public RectTransform FillRect;
        public TextMeshProUGUI Label;
    }

    public static RectTransform CreatePanel(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 sizeDelta, Color color)
    {
        GameObject panelObject = new(name);
        panelObject.transform.SetParent(parent, false);

        Image image = panelObject.AddComponent<Image>();
        image.color = color;

        RectTransform rectTransform = panelObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta = sizeDelta;
        rectTransform.anchoredPosition = Vector2.zero;
        return rectTransform;
    }

    public static TextMeshProUGUI CreateText(Transform parent, string name, string value, int fontSize, TextAlignmentOptions alignment)
    {
        GameObject textObject = new(name);
        textObject.transform.SetParent(parent, false);

        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
        text.text = value;
        text.fontSize = fontSize;
        text.alignment = alignment;
        text.color = Color.white;

        RectTransform rectTransform = text.rectTransform;
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        return text;
    }

    public static Button CreateButton(Transform parent, string name, string label, UnityAction onClick)
    {
        GameObject buttonObject = new(name);
        buttonObject.transform.SetParent(parent, false);

        Image image = buttonObject.AddComponent<Image>();
        image.color = new Color(0.18f, 0.22f, 0.28f, 0.96f);

        Button button = buttonObject.AddComponent<Button>();
        button.targetGraphic = image;
        button.onClick.AddListener(onClick);

        GameObject textObject = new("Label");
        textObject.transform.SetParent(buttonObject.transform, false);
        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
        text.text = label;
        text.fontSize = 28;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;

        RectTransform textRect = text.rectTransform;
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        return button;
    }

    public static ProgressBarRefs CreateProgressBar(Transform parent, string name, Color backgroundColor, Color fillColor)
    {
        GameObject rootObject = new(name);
        rootObject.transform.SetParent(parent, false);
        rootObject.AddComponent<RectTransform>();

        GameObject backgroundObject = new("Background");
        backgroundObject.transform.SetParent(rootObject.transform, false);
        Image background = backgroundObject.AddComponent<Image>();
        background.color = backgroundColor;

        RectTransform backgroundRect = background.rectTransform;
        backgroundRect.anchorMin = Vector2.zero;
        backgroundRect.anchorMax = Vector2.one;
        backgroundRect.offsetMin = Vector2.zero;
        backgroundRect.offsetMax = Vector2.zero;

        GameObject fillObject = new("Fill");
        fillObject.transform.SetParent(backgroundObject.transform, false);
        Image fill = fillObject.AddComponent<Image>();
        fill.color = fillColor;

        RectTransform fillRect = fill.rectTransform;
        fillRect.anchorMin = new Vector2(0f, 0f);
        fillRect.anchorMax = new Vector2(1f, 1f);
        fillRect.pivot = new Vector2(0f, 0.5f);
        fillRect.offsetMin = new Vector2(2f, 2f);
        fillRect.offsetMax = new Vector2(-2f, -2f);

        GameObject labelObject = new("Label");
        labelObject.transform.SetParent(backgroundObject.transform, false);
        TextMeshProUGUI label = labelObject.AddComponent<TextMeshProUGUI>();
        label.fontSize = 20;
        label.alignment = TextAlignmentOptions.Center;
        label.color = Color.white;

        RectTransform labelRect = label.rectTransform;
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = Vector2.zero;
        labelRect.offsetMax = Vector2.zero;

        return new ProgressBarRefs
        {
            Fill = fill,
            FillRect = fillRect,
            Label = label
        };
    }
}
