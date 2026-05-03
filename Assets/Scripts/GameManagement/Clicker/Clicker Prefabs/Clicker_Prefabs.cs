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
    [SerializeField] Prefab_PPS prefabPPS;
    [SerializeField] Prefab_WorkerList prefabWorkerList;
    [SerializeField] Prefab_Achievement prefabAchievement;

    System_Data data;

    void Awake()
    {
        data = Object.FindFirstObjectByType<System_Data>();
    }

    public void UpdateAllPrefabs(double totalPoints, int totalPPS)
    {
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

        if (prefabGoldenDropText != null && data != null)
        {
            prefabGoldenDropText.UpdateTotalGoldenDropsPrefab(data.goldenDrops);
        }

        if (prefabSkinsCounter != null && System_Wardrobe.Instance != null)
        {
            int total = System_Wardrobe.Instance.GetAllSkins().Count;
            int unlocked = System_Wardrobe.Instance.GetUnlockedSkinsCount();

            prefabSkinsCounter.UpdateSkinsPrefab(unlocked, total);
        }

        if (prefabLuckyBonus != null && data != null)
        {
            prefabLuckyBonus.UpdateTotalLuckyBonusesPrefab(data.luckyBonus);
        }

        if (prefabComboChain != null && data != null)
        {
            prefabComboChain.UpdateMaxComboPrefab(data.highestComboMultiplier);
        }

        if (prefabPPS != null)
        {
            prefabPPS.UpdatePPS(data.pointsPerSecond);
        }

        if (prefabWorkerList != null && data != null)
        {
            prefabWorkerList.UpdateWorkerList(data.workerLevels);
        }

        if (prefabAchievement != null && data != null && System_Achievements.Instance != null)
        {
            int unlocked = data.unlockedAchievementIDs.Count;
            int total = System_Achievements.Instance.GetAllAchievements().Count;

            prefabAchievement.UpdateAchievementsPrefab(unlocked, total);
        }
    }
}
