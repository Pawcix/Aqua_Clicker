using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTip_Achievement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float delay = 0.5f;

    Achievement achievementData;
    Coroutine delayCoroutine;
    bool isUnlocked;
    bool isMouseOver = false;

    public void SetupTooltip(Achievement ach, bool unlocked)
    {
        achievementData = ach;
        isUnlocked = unlocked;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (achievementData == null) return;
        isMouseOver = true;

        if (delayCoroutine != null) StopCoroutine(delayCoroutine);
        delayCoroutine = StartCoroutine(WaitAndShow());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        if (delayCoroutine != null) StopCoroutine(delayCoroutine);

        if (UI_ToolTipAchievements.Instance != null)
        {
            UI_ToolTipAchievements.Instance.HideTooltip();
        }
    }

    IEnumerator WaitAndShow()
    {
        yield return new WaitForSecondsRealtime(delay);

        if (isMouseOver && UI_ToolTipAchievements.Instance != null && achievementData != null)
        {
            UI_ToolTipAchievements.Instance.ShowTooltip(
                achievementData.title,
                achievementData.description,
                isUnlocked,
                achievementData.type,
                achievementData.requiredValue
            );
        }
    }

    void OnDisable()
    {
        isMouseOver = false;
        if (delayCoroutine != null) StopCoroutine(delayCoroutine);
        if (UI_ToolTipAchievements.Instance != null) UI_ToolTipAchievements.Instance.HideTooltip();
    }
}