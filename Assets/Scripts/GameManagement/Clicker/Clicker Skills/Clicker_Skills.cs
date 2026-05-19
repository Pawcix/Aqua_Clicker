using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Clicker_Skills : MonoBehaviour
{
    [SerializeField] public System_Data data;

    [Header("Visual Scripts:")]
    [SerializeField] Skill_AutoClick autoClickSkill;
    [SerializeField] Skill_AntiCheat antiCheatSkill;
    [SerializeField] Skill_AutoCollector autoCollectorSkill;
    [SerializeField] Skill_AutoLuckyBonusCollector autoluckyCollectorSkill;
    [SerializeField] Skill_Multiplier[] multiplierSkills;

    [Header("Abilities Unlock Levels (Permanent Mastery LVL):")]
    [SerializeField] int unlockAutoClickLvl = 20;
    [SerializeField] int unlockAntiCheatLvl = 50;
    [SerializeField] int unlockAutoColLvl = 80;
    [SerializeField] int unlockLuckyColLvl = 150;

    [Header("Required Level Text Elements:")]
    [SerializeField] TextMeshProUGUI reqAutoClickText;
    [SerializeField] TextMeshProUGUI reqAntiCheatText;
    [SerializeField] TextMeshProUGUI reqAutoColText;
    [SerializeField] TextMeshProUGUI reqLuckyColText;

    [Header("Visual Colors for Locked State:")]
    [SerializeField] Color lockedColor = new Color(0.3f, 0.3f, 0.3f, 0.6f);
    [SerializeField] Color unlockedColor = Color.white;

    public bool isAntiCheatBypassActive { get => data.isAntiCheatBypassActive; set => data.isAntiCheatBypassActive = value; }
    public int currentSkinIndex { get => data.currentSkinIndex; set => data.currentSkinIndex = value; }
    public bool isAutoClickerActive { get => data.isAutoClickerActive; set => data.isAutoClickerActive = value; }
    public bool isAutoCollectorActive { get => data.isAutoCollectorActive; set => data.isAutoCollectorActive = value; }
    public bool isLuckyCollectorActive { get => data.isLuckyCollectorActive; set => data.isLuckyCollectorActive = value; }

    public void UpdateAllSkills(int totalPoints) { }

    void Start()
    {
        RefreshSkillsVisuals();
    }

    public void RefreshMultiplierButtons()
    {
        if (data == null) return;

        if (multiplierSkills == null || multiplierSkills.Length == 0)
        {
            multiplierSkills = GetComponentsInChildren<Skill_Multiplier>(true);
        }

        foreach (Skill_Multiplier sm in multiplierSkills)
        {
            if (sm == null) continue;

            int requiredAwayLvl = (sm.multiplierValue - 1) * 20;

            bool isUnlocked = data.awayMasteryLvl >= requiredAwayLvl;

            if (sm.multiplierButton != null)
            {
                sm.multiplierButton.interactable = isUnlocked;

                if (sm.multiplierButton.image != null)
                {
                    sm.multiplierButton.image.color = isUnlocked ? unlockedColor : lockedColor;
                }
            }

            if (sm.reqText != null)
            {
                if (isUnlocked)
                {
                    sm.reqText.gameObject.SetActive(false);
                }
                else
                {
                    sm.reqText.gameObject.SetActive(true);
                    sm.reqText.text = $"REQ Mastery Income: \n{requiredAwayLvl} LEV";
                }
            }

            sm.RefreshVisuals();
        }
    }

    public void RefreshSkillsVisuals()
    {
        if (data == null) return;
        var wardrobe = Object.FindFirstObjectByType<System_Wardrobe>();
        if (wardrobe != null) wardrobe.LoadSkin(data.currentSkinIndex);

        if (autoClickSkill != null && autoClickSkill.autoClickButton != null)
        {
            bool unlocked = CheckAbilityUnlock(autoClickSkill.autoClickButton, unlockAutoClickLvl, reqAutoClickText);
            if (unlocked) autoClickSkill.SetAutoClickState(data.isAutoClickerActive);
        }

        if (antiCheatSkill != null && antiCheatSkill.bypassButton != null)
        {
            bool unlocked = CheckAbilityUnlock(antiCheatSkill.bypassButton, unlockAntiCheatLvl, reqAntiCheatText);
            if (unlocked) antiCheatSkill.SetBypassState(data.isAntiCheatBypassActive);
        }

        if (autoCollectorSkill != null && autoCollectorSkill.collectorButton != null)
        {
            bool unlocked = CheckAbilityUnlock(autoCollectorSkill.collectorButton, unlockAutoColLvl, reqAutoColText);
            if (unlocked) autoCollectorSkill.SetAutoCollectorState(data.isAutoCollectorActive);
        }

        if (autoluckyCollectorSkill != null && autoluckyCollectorSkill.collectorButton != null)
        {
            bool unlocked = CheckAbilityUnlock(autoluckyCollectorSkill.collectorButton, unlockLuckyColLvl, reqLuckyColText);
            if (unlocked) autoluckyCollectorSkill.SetLuckyCollectorState(data.isLuckyCollectorActive);
        }

        RefreshMultiplierButtons();
    }

    bool CheckAbilityUnlock(Button btn, int requiredLvl, TextMeshProUGUI reqText)
    {
        if (btn == null || data == null) return false;

        bool isUnlocked = data.clickMasteryLvl >= requiredLvl;

        btn.interactable = isUnlocked;

        if (btn.image != null)
        {
            btn.image.color = isUnlocked ? unlockedColor : lockedColor;
        }

        if (reqText != null)
        {
            if (isUnlocked)
            {
                reqText.gameObject.SetActive(false);
            }
            else
            {
                reqText.gameObject.SetActive(true);
                reqText.text = $"REQ Click Mastery: \n{requiredLvl} LEV";
            }
        }

        return isUnlocked;
    }
}