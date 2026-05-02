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
    [SerializeField] System_AntiCheat antiCheat;
    [SerializeField] System_CPS cpsSystem;
    [SerializeField] System_WordsEffect clickWords;

    float uiUpdateTimer = 0f;
    float uiUpdateInterval = 0.1f;

    void Update()
    {
        uiUpdateTimer += Time.deltaTime;
        if (uiUpdateTimer >= uiUpdateInterval)
        {
            UpdateHeavyUI();
            uiUpdateTimer = 0f;
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
        if (antiCheat != null && !antiCheat.CheckClickLegal()) return;
        if (cpsSystem != null) cpsSystem.OnClickRegistered();

        double pointsFromThisClick = (double)data.pointsPerClick * data.clickMultiplier;

        if (data.isGoldRushActive)
        {
            pointsFromThisClick *= 2.0;
        }

        addSystem.AddPoints();

        if (ComboChain.Instance != null)
        {
            ComboChain.Instance.OnClickRegistered(pointsFromThisClick);
        }

        if (PointsDisplay.Instance != null) PointsDisplay.Instance.Pulse();
        if (clickWords != null) clickWords.ShowRandomWord();
    }
}
