using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip_LevelManager : MonoBehaviour
{
    public static ToolTip_LevelManager Instance;

    [Header("UI Elements:")]
    [SerializeField] GameObject tooltipWindow;
    [SerializeField] TextMeshProUGUI tooltipText;
    [SerializeField] RectTransform windowRect;
    [SerializeField] RectTransform canvasRect;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (tooltipWindow != null)
        {
            tooltipWindow.SetActive(false);
        }
    }

    public void ShowTooltip(string text, Vector3 buttonPosition)
    {
        if (tooltipText == null || tooltipWindow == null) return;

        tooltipText.text = text;
        tooltipWindow.SetActive(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate(windowRect);
        PositionTooltip();
    }

    public void HideTooltip()
    {
        if (tooltipWindow != null)
        {
            tooltipWindow.SetActive(false);
        }
    }

    void PositionTooltip()
    {
        if (windowRect == null || canvasRect == null) return;

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

    void Update()
    {
        if (tooltipWindow != null && tooltipWindow.activeSelf)
        {
            PositionTooltip();
        }
    }

    public void UpdateDynamicContent(string newText)
    {
        if (tooltipWindow != null && tooltipWindow.activeSelf)
        {
            if (tooltipText != null) tooltipText.text = newText;
            LayoutRebuilder.ForceRebuildLayoutImmediate(windowRect);
        }
    }
}
