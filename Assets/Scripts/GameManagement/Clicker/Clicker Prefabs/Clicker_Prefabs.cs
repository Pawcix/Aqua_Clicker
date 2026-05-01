using UnityEngine;

public class Clicker_Prefabs : MonoBehaviour
{
    [SerializeField] Prefab_Points prefabTotalPointsText;
    [SerializeField] Prefab_Timer prefabTimerText;
    [SerializeField] Total_PPS prefabPPSText;
    [SerializeField] Prefab_AwayIncome prefabAwayIncomeText;
    [SerializeField] Prefab_GoldenDrop prefabGoldenDropText;
    [SerializeField] Prefab_Skins prefabSkinsCounter;
    [SerializeField] Prefab_LuckyBonus prefabLuckyBonus;

    System_Data data;

    void Awake()
    {
        data = Object.FindFirstObjectByType<System_Data>();
    }

    public void UpdateAllPrefabs(int totalPoints, int totalPPS)
    {
        if (prefabTotalPointsText != null)
        {
            prefabTotalPointsText.UpdateTotalPointsPrefab(totalPoints);
        }

        if (prefabTimerText != null)
        {
            prefabTimerText.UpdateTimerPrefab();
        }

        if (prefabPPSText != null)
        {
            prefabPPSText.UpdatePPS(totalPPS);
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
    }
}
