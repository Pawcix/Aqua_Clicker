using UnityEngine;

public class Clicker_Prefabs : MonoBehaviour
{
    [SerializeField] Prefab_Points prefabTotalPointsText;
    [SerializeField] Prefab_Timer prefabTimerText;
    [SerializeField] Prefab_AwayIncome prefabAwayIncomeText;
    [SerializeField] Prefab_GoldenDrop prefabGoldenDropText;
    [SerializeField] Prefab_Skins prefabSkinsCounter;
    [SerializeField] Prefab_LuckyBonus prefabLuckyBonus;
    [SerializeField] Prefab_ComboChain prefabComboChain;
    [SerializeField] Prefab_ComboCrit prefabComboCrit;
    [SerializeField] Prefab_PPS prefabPPS;
    [SerializeField] Prefab_WorkerList prefabWorkerList;
    [SerializeField] Prefab_Achievement prefabAchievement;
    [SerializeField] Prefab_DailyStreak prefabDailyStreak;
    [SerializeField] Prefab_Rebirth prefabRebirthsCounter;

    System_Data data;

    void Awake()
    {
        data = Object.FindFirstObjectByType<System_Data>();
    }

    public void UpdateAllPrefabs(double totalPoints, double totalPPS)
    {
        if (data == null) return;

        if (prefabTotalPointsText != null)
        {
            prefabTotalPointsText.UpdateTotalPointsPrefab(totalPoints);
        }

        if (prefabTimerText != null)
        {
            prefabTimerText.UpdateTimerPrefab();
        }

        if (prefabAwayIncomeText != null)
        {
            prefabAwayIncomeText.UpdateTotalDisplay();
        }

        if (prefabGoldenDropText != null)
        {
            prefabGoldenDropText.UpdateTotalGoldenDropsPrefab(data.goldenDrops);
        }

        if (prefabSkinsCounter != null && System_Wardrobe.Instance != null)
        {
            int total = System_Wardrobe.Instance.GetAllSkins().Count;
            int unlocked = System_Wardrobe.Instance.GetUnlockedSkinsCount();
            prefabSkinsCounter.UpdateSkinsPrefab(unlocked, total);
        }

        if (prefabLuckyBonus != null)
        {
            prefabLuckyBonus.UpdateTotalLuckyBonusesPrefab(data.luckyBonus);
        }

        if (prefabRebirthsCounter != null)
        {
            prefabRebirthsCounter.UpdateTotalRebirthsPrefab(data.rebirthCount);
        }

        if (prefabComboChain != null)
        {
            prefabComboChain.UpdateMaxComboPrefab(data.highestComboMultiplier);
        }

        if (prefabComboCrit != null)
        {
            prefabComboCrit.UpdateMaxComboPrefab(data.highestCritMultiplier);
        }

        if (prefabPPS != null)
        {
            double realPPS = (data.basePPS + data.workersPPS) * data.currentDailyMultiplier;

            if (data.isGoldRushActive) realPPS *= 2.0;

            prefabPPS.UpdatePPS(realPPS);
        }

        if (prefabWorkerList != null)
        {
            prefabWorkerList.UpdateWorkerList(data.workerLevels);
        }

        if (prefabDailyStreak != null)
        {
            prefabDailyStreak.UpdateDailyStreakPrefab(data.loginStreak);
        }

        if (prefabAchievement != null && System_Achievements.Instance != null)
        {
            int unlocked = data.unlockedAchievementIDs.Count;
            int total = System_Achievements.Instance.GetAllAchievements().Count;
            prefabAchievement.UpdateAchievementsPrefab(unlocked, total);
        }
    }
}
