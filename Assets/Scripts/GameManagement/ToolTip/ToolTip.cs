using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    public static ToolTip Instance;
    public TextMeshProUGUI tooltipText;
    public RectTransform backgroundRect;

    private void Awake()
    {
        Instance = this;

        Canvas canvas = GetComponent<Canvas>();
        if (canvas == null) canvas = gameObject.AddComponent<Canvas>();

        canvas.overrideSorting = true;
        canvas.sortingOrder = 20;
        if (GetComponent<GraphicRaycaster>() == null) gameObject.AddComponent<GraphicRaycaster>();

        CanvasGroup cg = GetComponent<CanvasGroup>();
        if (cg == null) cg = gameObject.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = false;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.position = Input.mousePosition + new Vector3(15f, -15f, 0f);
    }

    public void ShowTooltip(string text)
    {
        gameObject.SetActive(true);
        tooltipText.text = text;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
