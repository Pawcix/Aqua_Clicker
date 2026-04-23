using UnityEngine;
using UnityEngine.UI;

public class Skill_AutoClick : MonoBehaviour
{
    [SerializeField] Clicker_Skills masterSkills;
    [SerializeField] Button autoClickButton;
    [SerializeField] Image autoClickButtonImage;
    [SerializeField] Sprite neutralIcon;
    [SerializeField] Sprite activeIcon;
    [SerializeField] float clickInterval = 0.5f;

    Clicker_System _clickerSystem;

    void Start()
    {
        autoClickButton.onClick.AddListener(ToggleAutoClicker);
        _clickerSystem = Object.FindFirstObjectByType<Clicker_System>();
    }

    void ToggleAutoClicker()
    {
        if (masterSkills == null) return;
        SetAutoClickState(!masterSkills.isAutoClickerActive);
    }

    public void SetAutoClickState(bool active)
    {
        if (masterSkills == null) return;

        masterSkills.isAutoClickerActive = active;
        autoClickButtonImage.sprite = active ? activeIcon : neutralIcon;

        CancelInvoke(nameof(AutoClick));
        if (active) InvokeRepeating(nameof(AutoClick), clickInterval, clickInterval);
    }

    void AutoClick()
    {
        if (_clickerSystem != null) _clickerSystem.Click();
    }
}
