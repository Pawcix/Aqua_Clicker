using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip_AbilitiesManager : MonoBehaviour
{
    public static ToolTip_AbilitiesManager Instance;

    [Header("UI Elements:")]
    [SerializeField] GameObject tooltipWindow;
    [SerializeField] TextMeshProUGUI tooltipText;
    [SerializeField] RectTransform windowRect;
    [SerializeField] RectTransform canvasRect;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        Canvas canvas = GetComponent<Canvas>();
        if (canvas == null)
        {
            canvas = gameObject.AddComponent<Canvas>();
        }

        canvas.overrideSorting = true;
        canvas.sortingOrder = 101;

        if (GetComponent<GraphicRaycaster>() == null)
        {
            gameObject.AddComponent<GraphicRaycaster>();
        }

        if (tooltipWindow != null) tooltipWindow.SetActive(false);
    }

    public void ShowTooltip(string text)
    {
        if (tooltipWindow == null || tooltipText == null) return;

        tooltipText.text = text;
        tooltipWindow.SetActive(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate(windowRect);
        PositionTooltip();
    }

    public void HideTooltip()
    {
        if (tooltipWindow != null) tooltipWindow.SetActive(false);
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
}