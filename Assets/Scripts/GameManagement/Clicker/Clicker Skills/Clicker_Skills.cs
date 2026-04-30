using UnityEngine;

public class Clicker_Skills : MonoBehaviour
{
    [SerializeField] public System_Data data;

    [Header("Visual Scripts:")]
    [SerializeField] Skill_AutoClick autoClickSkill;
    [SerializeField] Skill_AntiCheat antiCheatSkill;
    [SerializeField] Skill_AutoCollector autoCollectorSkill;
    [SerializeField] Skill_Multiplier[] multiplierSkills;


    public bool isAntiCheatBypassActive
    {
        get => data.isAntiCheatBypassActive;
        set => data.isAntiCheatBypassActive = value;
    }

    public int currentSkinIndex
    {
        get => data.currentSkinIndex;
        set => data.currentSkinIndex = value;
    }

    public bool isAutoClickerActive
    {
        get => data.isAutoClickerActive;
        set => data.isAutoClickerActive = value;
    }

    public bool isAutoCollectorActive
    {
        get => data.isAutoCollectorActive;
        set => data.isAutoCollectorActive = value;
    }

    public void UpdateAllSkills(int totalPoints) { }

    public void RefreshMultiplierButtons()
    {
        foreach (var skill in multiplierSkills)
        {
            if (skill != null) skill.RefreshVisuals();
        }
    }

    public void RefreshSkillsVisuals()
    {
        if (data == null) return;
        var wardrobe = Object.FindFirstObjectByType<System_Wardrobe>();
        if (wardrobe != null) wardrobe.LoadSkin(data.currentSkinIndex);
        if (autoClickSkill != null) autoClickSkill.SetAutoClickState(data.isAutoClickerActive);
        if (antiCheatSkill != null) antiCheatSkill.SetBypassState(data.isAntiCheatBypassActive);
        RefreshMultiplierButtons();
        if (autoCollectorSkill != null) autoCollectorSkill.SetAutoCollectorState(data.isAutoCollectorActive);
    }
}
