using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UI_StatTooltipItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] string statTitle = "Game Time";
    [SerializeField] TextMeshProUGUI valueTextSource;
    [SerializeField] float delay = 0.2f;

    Coroutine delayCoroutine;
    bool isMouseOver = false;
    bool isTooltipVisible = false;

    void Update()
    {
        if (isMouseOver && isTooltipVisible && ToolTip_AbilitiesManager.Instance != null)
        {
            ToolTip_AbilitiesManager.Instance.ShowTooltip(BuildTooltipText());
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
        if (delayCoroutine != null) StopCoroutine(delayCoroutine);
        delayCoroutine = StartCoroutine(WaitAndShow());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        isTooltipVisible = false;
        if (delayCoroutine != null) StopCoroutine(delayCoroutine);

        if (ToolTip_AbilitiesManager.Instance != null)
        {
            ToolTip_AbilitiesManager.Instance.HideTooltip();
        }
    }

    IEnumerator WaitAndShow()
    {
        yield return new WaitForSeconds(delay);

        if (isMouseOver && ToolTip_AbilitiesManager.Instance != null)
        {
            ToolTip_AbilitiesManager.Instance.ShowTooltip(BuildTooltipText());
            isTooltipVisible = true;
        }
    }

    string BuildTooltipText()
    {
        string currentValue = (valueTextSource != null) ? valueTextSource.text : "N/A";
        currentValue = currentValue.Replace("\n", " ").Trim();

        string text = "";
        text += $"<color=#FFBF00><size=110%>{statTitle}</size></color>\n";
        text += $"{currentValue}\n";

        return text;
    }

    void OnDisable()
    {
        isMouseOver = false;
        isTooltipVisible = false;
        if (ToolTip_AbilitiesManager.Instance != null)
        {
            ToolTip_AbilitiesManager.Instance.HideTooltip();
        }
    }
}