using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTip_AbilityItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum AbilityType { GeneralLevel }

    [SerializeField] AbilityType type = AbilityType.GeneralLevel;
    [SerializeField] int requiredLevel = 20;
    [SerializeField] string abilityName = "Auto Clicker";
    [SerializeField] float delay = 0.3f;

    Clicker_Skills skillsSystem;
    Coroutine delayCoroutine;
    bool isMouseOver = false;

    void Start()
    {
        skillsSystem = Object.FindAnyObjectByType<Clicker_Skills>();
    }

    bool IsUnlocked()
    {
        if (skillsSystem == null || skillsSystem.data == null) return false;
        return skillsSystem.data.currentLevel >= requiredLevel;
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
        }
    }

    string BuildTooltipText()
    {
        if (skillsSystem == null || skillsSystem.data == null) return "Ability Info";

        bool unlocked = IsUnlocked();
        string text = "";

        if (unlocked)
        {
            text += $"{abilityName}\n";
            text += $"<color=#00FF00><b>UNLOCKED</b></color>\n";
        }
        else
        {
            text += $"<color=#FF4444>{abilityName} (LOCKED)</color>\n";
            text += $"Requires: <color=#FFBF00>{requiredLevel} LVL</color>\n";
        }

        text += $"<size=80%>Your Current Level: {skillsSystem.data.currentLevel} LVL</size>";

        return text;
    }

    void OnDisable()
    {
        isMouseOver = false;
        if (ToolTip_AbilitiesManager.Instance != null) ToolTip_AbilitiesManager.Instance.HideTooltip();
    }
}