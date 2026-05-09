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

    [Header("Anti-Cheat:")]
    [SerializeField] AntiCheat_Clicks antiClick;
    [SerializeField] AntiCheat_Break antiBreak;

    float uiUpdateTimer = 0f;
    float uiUpdateInterval = 0.1f;

    void Update()
    {
        if (data != null && data.pointsPerSecond > 0)
        {
            double ppsThisFrame = data.pointsPerSecond * Time.deltaTime;

            data.pointsCounterFloat += ppsThisFrame;

            if (System_Leveling.Instance != null)
            {
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
        float currentPPS = data.pointsPerSecond;
        int displayPPS = (int)System.Math.Floor(currentPPS);

        if (clickerPrefabs != null)
            clickerPrefabs.UpdateAllPrefabs(currentTotal, displayPPS);

        if (clickerSkills != null)
        {
            int skillsValue = currentTotal > int.MaxValue ? int.MaxValue : (int)currentTotal;
            clickerSkills.UpdateAllSkills(skillsValue);
        }

        if (clickerStats != null)
            clickerStats.UpdateAllStats(currentTotal, displayPPS);
    }

    public void Click()
    {
        if (addSystem == null || data == null) return;
        if (antiClick != null && !antiClick.CheckClickLegal()) return;

        double basePoints = (double)data.pointsPerClick * data.clickMultiplier;
        if (data.isGoldRushActive) basePoints *= 2.0;

        bool isCrit;
        double finalPoints = System_Critical.Instance.CalculateCriticalDamage(basePoints, out isCrit);

        System_Economy.Instance.AddPoints(finalPoints);

        if (System_Leveling.Instance != null)
            System_Leveling.Instance.RegisterPointGain(finalPoints);

        if (isCrit)
        {

        }
        else
        {
            if (clickWords != null) clickWords.ShowRandomWord();
        }

        if (ComboChain.Instance != null) ComboChain.Instance.OnClickRegistered(finalPoints);
        if (PointsDisplay.Instance != null) PointsDisplay.Instance.Pulse();
        if (System_Achievements.Instance != null) System_Achievements.Instance.CheckAchievements();
        if (clickWords != null && isCrit == false) { }
    }
}
