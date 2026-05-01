using UnityEngine;
using UnityEngine.Events;

public class Clicker_System : MonoBehaviour
{
    public static UnityEvent<int, int> OnItemBought = new UnityEvent<int, int>();

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

        int currentTotal = Mathf.RoundToInt(data.pointsCounterFloat);
        int currentPPS = data.pointsPerSecond;

        if (clickerPrefabs != null) clickerPrefabs.UpdateAllPrefabs(currentTotal, currentPPS);
        if (clickerStats != null) clickerStats.UpdateAllStats(currentTotal, currentPPS);
        if (clickerSkills != null) clickerSkills.UpdateAllSkills(currentTotal);
    }

    public void Click()
    {
        if (addSystem == null || data == null) return;
        if (antiCheat != null && !antiCheat.CheckClickLegal()) return;
        if (cpsSystem != null) cpsSystem.OnClickRegistered();

        addSystem.AddPoints();

        if (pointsDisplay != null) pointsDisplay.Pulse();
        if (clickWords != null) clickWords.ShowRandomWord();
    }
}
