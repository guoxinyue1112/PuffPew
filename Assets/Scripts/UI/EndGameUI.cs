using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    private GameManager gameManager;
    private RectTransform panel;
    private TextMeshProUGUI titleText;
    private TextMeshProUGUI bodyText;

    public void BuildUI()
    {
        panel = UIFactory.CreatePanel(
            transform,
            "EndGamePanel",
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(560f, 280f),
            new Color(0.05f, 0.07f, 0.10f, 0.95f));

        titleText = UIFactory.CreateText(panel, "Title", string.Empty, 38, TextAlignmentOptions.Center);
        RectTransform titleRect = titleText.rectTransform;
        titleRect.anchorMin = new Vector2(0f, 1f);
        titleRect.anchorMax = new Vector2(1f, 1f);
        titleRect.pivot = new Vector2(0.5f, 1f);
        titleRect.anchoredPosition = new Vector2(0f, -34f);
        titleRect.sizeDelta = new Vector2(-48f, 54f);

        bodyText = UIFactory.CreateText(panel, "Body", string.Empty, 24, TextAlignmentOptions.Center);
        RectTransform bodyRect = bodyText.rectTransform;
        bodyRect.anchorMin = new Vector2(0f, 1f);
        bodyRect.anchorMax = new Vector2(1f, 1f);
        bodyRect.pivot = new Vector2(0.5f, 1f);
        bodyRect.anchoredPosition = new Vector2(0f, -108f);
        bodyRect.sizeDelta = new Vector2(-48f, 60f);

        Button restartButton = UIFactory.CreateButton(panel, "RestartButton", "Restart", () => gameManager.RestartGame());
        RectTransform restartRect = restartButton.GetComponent<RectTransform>();
        restartRect.anchorMin = new Vector2(0.5f, 0f);
        restartRect.anchorMax = new Vector2(0.5f, 0f);
        restartRect.pivot = new Vector2(0.5f, 0f);
        restartRect.anchoredPosition = new Vector2(0f, 24f);
        restartRect.sizeDelta = new Vector2(240f, 70f);

        panel.gameObject.SetActive(false);
    }

    public void Initialize(GameManager manager)
    {
        gameManager = manager;
    }

    public void ShowGameOver()
    {
        titleText.text = "GAME OVER";
        bodyText.text = string.Empty;
        panel.gameObject.SetActive(true);
    }

    public void ShowVictory()
    {
        titleText.text = "VICTORY";
        bodyText.text = "You completed all 20 waves.";
        panel.gameObject.SetActive(true);
    }
}
