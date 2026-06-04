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
        if (delayCoroutine != null) StopCoroutine(delayCoroutine);
        delayCoroutine = StartCoroutine(WaitAndShow());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        if (delayCoroutine != null) StopCoroutine(delayCoroutine);

        if (ToolTip_LevelManager.Instance != null)
        {
            ToolTip_LevelManager.Instance.HideTooltip();
        }
    }

    IEnumerator WaitAndShow()
    {
        yield return new WaitForSeconds(delay);
        if (isMouseOver && ToolTip_LevelManager.Instance != null)
        {
            ToolTip_LevelManager.Instance.ShowTooltip(GetLevelingInfo(), transform.position);
        }
    }

    void Update()
    {
        if (isMouseOver && ToolTip_LevelManager.Instance != null)
        {
            ToolTip_LevelManager.Instance.UpdateDynamicContent(GetLevelingInfo());
        }
    }

    string GetLevelingInfo()
    {
        if (data == null) return "Loading...";

        double progressPercent = 0;
        if (data.xpToNextLevel > 0)
        {
            progressPercent = (data.currentXP / data.xpToNextLevel) * 100;
        }

        string info = $"<color=#FFA500>Level: {data.currentLevel}</color>\n";
        info += $"";
        info += $"XP: {NumberFormatter.FormatWithDots(data.currentXP)} / {NumberFormatter.FormatWithDots(data.xpToNextLevel)}\n";
        info += $"Progress: {progressPercent:F1}%\n";

        return info;
    }
}