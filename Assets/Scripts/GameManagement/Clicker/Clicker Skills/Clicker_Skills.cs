using UnityEngine;

public class Clicker_Skills : MonoBehaviour
{
    [Header("Visual Scripts:")]
    [SerializeField] Skill_Skin skinSkill;
    [SerializeField] Skill_AutoClick autoClickSkill;
    [SerializeField] Skill_AntiCheat antiCheatSkill;

    [SerializeField] public System_Data data;
    
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

    public void UpdateAllSkills(int totalPoints) { }

    public void RefreshSkillsVisuals()
    {
        if (data == null) return;
        if (skinSkill != null) skinSkill.LoadSkin(data.currentSkinIndex);
        if (autoClickSkill != null) autoClickSkill.SetAutoClickState(data.isAutoClickerActive);
        if (antiCheatSkill != null) antiCheatSkill.SetBypassState(data.isAntiCheatBypassActive);
    }
}
