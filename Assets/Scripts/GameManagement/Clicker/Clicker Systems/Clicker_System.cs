using UnityEngine;
using UnityEngine.Events;

public class Clicker_System : MonoBehaviour
{
    public static UnityEvent<double, float> OnItemBought = new UnityEvent<double, float>();

    [Header("Data Source:")]
    [SerializeField] System_Data data;

    [Header("Stats & Prefabs:")]
    [SerializeField] Clicker_Prefabs clickerPrefabs;
    [SerializeField] Clicker_Skills clickerSkills;
    [SerializeField] Clicker_Stats clickerStats;

    [Header("Visuals:")]
    [SerializeField] PointsDisplay pointsDisplay;

    [Header("Systems:")]
    [SerializeField] System_Add addSystem;
    [SerializeField] System_CPS cpsSystem;
    [SerializeField] System_WordsEffect clickWords;
    [SerializeField] System_Achievements achievementSystem;
    [SerializeField] System_WardrobeUnlockSkin wardrobeUnlockSkin;
    [SerializeField] System_Critical criticalSystem;
    [SerializeField] System_CriticalEffect criticalEffect;

    [Header("Anti-Cheat:")]
    [SerializeField] AntiCheat_Clicks antiClick;
    [SerializeField] AntiCheat_Break antiBreak;

    float uiUpdateTimer = 0f;
    float uiUpdateInterval = 0.1f;

    void Update()
    {
        if (data != null)
        {
            if (data.wheelBonusTimer > 0)
            {
                data.wheelBonusTimer -= Time.deltaTime;
                if (data.wheelBonusTimer <= 0)
                {
                    data.wheelBonusTimer = 0;
                    data.wheelMultiplier = 1.0f;
                }
            }

            double totalBasePPS = data.basePPS + data.workersPPS;
            double finalPPS = totalBasePPS * data.currentDailyMultiplier * data.wheelMultiplier;

            if (data.isGoldRushActive) finalPPS *= 2.0;

            if (finalPPS > 0)
            {
                double ppsThisFrame = finalPPS * Time.deltaTime;
                data.pointsCounterFloat += ppsThisFrame;

                if (System_Leveling.Instance != null)
                    System_Leveling.Instance.RegisterPointGain(ppsThisFrame);
            }
        }

        uiUpdateTimer += Time.deltaTime;
        if (uiUpdateTimer >= uiUpdateInterval)
        {
            UpdateHeavyUI();
            uiUpdateTimer = 0f;
        }

        if (achievementSystem != null && achievementSystem.IsReady())
        {
            achievementSystem.CheckAchievements();
        }
    }

    void UpdateHeavyUI()
    {
        if (data == null) return;

        double currentTotal = System.Math.Floor(data.pointsCounterFloat);
        double realPPS = (data.basePPS + data.workersPPS) * data.currentDailyMultiplier * data.wheelMultiplier;

        if (data.isGoldRushActive) realPPS *= 2.0;

        if (clickerPrefabs != null)
            clickerPrefabs.UpdateAllPrefabs(currentTotal, realPPS);

        if (clickerSkills != null)
        {
            int skillsValue = currentTotal > int.MaxValue ? int.MaxValue : (int)currentTotal;
            clickerSkills.UpdateAllSkills(skillsValue);
        }

        if (clickerStats != null)
            clickerStats.UpdateAllStats(currentTotal, (float)realPPS);
    }

    public void Click()
    {
        if (data == null) return;
        if (antiClick != null && !antiClick.CheckClickLegal()) return;
        if (cpsSystem != null) cpsSystem.OnClickRegistered();

        double basePoints = (double)data.pointsPerClick * data.clickMultiplier * data.wheelMultiplier;

        if (Mastery.Instance != null)
        {
            float masteryBonus = 1.0f + Mastery.Instance.GetMasteryBonus(Mastery.MasteryType.Click);
            basePoints *= masteryBonus;
            Mastery.Instance.AddMasteryXP(Mastery.MasteryType.Click, 1f);
        }

        basePoints *= data.currentDailyMultiplier;
        if (data.isGoldRushActive) basePoints *= 2.0;

        bool isCrit;
        double finalPoints = System_Critical.Instance.CalculateCriticalDamage(basePoints, out isCrit);

        if (isCrit)
        {
            if (CritComboChain.Instance != null)
                CritComboChain.Instance.OnCritRegistered(finalPoints);

            if (CritComboChain.Instance != null)
                finalPoints *= CritComboChain.Instance.GetCurrentMultiplier();

            if (Mastery.Instance != null)
                Mastery.Instance.AddMasteryXP(Mastery.MasteryType.Critical, 5f);
        }
        else
        {
            if (CritComboChain.Instance != null)
                CritComboChain.Instance.OnNormalClickRegistered();

            if (clickWords != null) clickWords.ShowRandomWord();
        }

        System_Economy.Instance.AddPoints(finalPoints);

        if (System_Leveling.Instance != null)
            System_Leveling.Instance.RegisterPointGain(finalPoints);

        if (ComboChain.Instance != null) ComboChain.Instance.OnClickRegistered(finalPoints);

        if (PointsDisplay.Instance != null) PointsDisplay.Instance.Pulse();
    }
}
