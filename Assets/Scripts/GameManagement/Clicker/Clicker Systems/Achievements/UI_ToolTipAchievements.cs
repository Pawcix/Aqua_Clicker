using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ToolTipAchievements : MonoBehaviour
{
    public static UI_ToolTipAchievements Instance;

    [Header("UI Elements:")]
    [SerializeField] GameObject tooltipWindow;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] RectTransform windowRect;
    [SerializeField] RectTransform canvasRect;

    [Header("New: Dynamic Background Card:")]
    [SerializeField] Image tooltipBackgroundDisplay;

    [Header("New: Card Background Sprites:")]
    [SerializeField] Sprite unlockedBackgroundCard;
    [SerializeField] Sprite lockedBackgroundCard;

    [Header("Global Data Reference:")]
    [SerializeField] System_Data data;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        tooltipWindow.SetActive(false);

        Canvas canvas = GetComponent<Canvas>();
        if (canvas == null) canvas = gameObject.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = 100;

        if (GetComponent<GraphicRaycaster>() == null) gameObject.AddComponent<GraphicRaycaster>();
    }

    public void ShowTooltip(string title, string description, bool isUnlocked, AchievementType type, double requiredValue)
    {
        titleText.text = $"<color=#FFD700>{title}</color>";

        if (tooltipBackgroundDisplay != null)
        {
            tooltipBackgroundDisplay.sprite = isUnlocked ? unlockedBackgroundCard : lockedBackgroundCard;
        }

        double currentProgress = 0;
        string unitName = "";

        if (data != null)
        {
            switch (type)
            {
                case AchievementType.TotalPoints:
                    currentProgress = data.pointsCounterFloat;
                    unitName = "pkt";
                    break;
                case AchievementType.GoldenDrops:
                    currentProgress = data.goldenDrops;
                    unitName = "golden drops";
                    break;
            }
        }

        if (isUnlocked) currentProgress = requiredValue;

        string formattedCurrent = NumberFormatter.FormatWithDots(currentProgress);
        string formattedRequired = NumberFormatter.FormatWithDots(requiredValue);
        string progressLine = $"Progress: {formattedCurrent} / {formattedRequired} {unitName}";

        descriptionText.text = $"{description}\n{progressLine}";
        tooltipWindow.SetActive(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate(windowRect);
        PositionTooltip();
    }

    public void HideTooltip()
    {
        tooltipWindow.SetActive(false);
    }

    void Update()
    {
        if (tooltipWindow.activeSelf)
        {
            PositionTooltip();
        }
    }

    void PositionTooltip()
    {
        Vector2 mousePos = Input.mousePosition;

        if (mousePos.y > Screen.height / 2f)
            windowRect.pivot = new Vector2(0.5f, 1.5f);
        else
            windowRect.pivot = new Vector2(0.5f, -0.1f);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            mousePos,
            Camera.main,
            out Vector2 localPoint
        );

        float halfWidth = windowRect.rect.width / 2f;
        float canvasHalfWidth = canvasRect.rect.width / 2f;
        localPoint.x = Mathf.Clamp(localPoint.x, -canvasHalfWidth + halfWidth, canvasHalfWidth - halfWidth);

        windowRect.anchoredPosition = localPoint;
        windowRect.localPosition = new Vector3(windowRect.localPosition.x, windowRect.localPosition.y, 0f);
        windowRect.localScale = Vector3.one;
    }
}