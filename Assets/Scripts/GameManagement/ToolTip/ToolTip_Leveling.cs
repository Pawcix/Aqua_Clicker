using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTip_Leveling : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float delay = 0.5f;
    [SerializeField] System_Data data;

    Coroutine delayCoroutine;
    bool isMouseOver = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
        delayCoroutine = StartCoroutine(WaitAndShow());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        if (delayCoroutine != null) StopCoroutine(delayCoroutine);
        ToolTip.Instance.HideTooltip();
    }

    IEnumerator WaitAndShow()
    {
        yield return new WaitForSeconds(delay);
        if (isMouseOver)
        {
            ToolTip.Instance.ShowTooltip(GetLevelingInfo(), transform.position);
        }
    }

    void Update()
    {
        if (isMouseOver && ToolTip.Instance != null)
        {
            ToolTip.Instance.UpdateDynamicContent(GetLevelingInfo());
        }
    }

    string GetLevelingInfo()
    {
        if (data == null) return "Loading...";

        double progressPercent = (data.currentXP / data.xpToNextLevel) * 100;

        string info = $"<color=#00BFFF>Level: {data.currentLevel}</color>\n";
        info += $"\n";
        info += $"XP: {NumberFormatter.FormatWithDots(data.currentXP)} / {NumberFormatter.FormatWithDots(data.xpToNextLevel)}\n";
        info += $"Progress: {progressPercent:F1}%\n";
        info += $"\n";
        info += $"<color=#FFD700>Rebirth Points: {data.rebirthPoints}</color>";

        return info;
    }
}
