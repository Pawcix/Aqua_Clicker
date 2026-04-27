using UnityEngine;

public class Clicker_Prefabs : MonoBehaviour
{
    [SerializeField] Prefab_Points prefabTotalPointsText;
    [SerializeField] Prefab_Timer prefabTimerText;
    [SerializeField] Total_PPS prefabPPSText;
    [SerializeField] Prefab_AwayIncome prefabAwayIncomeText;

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
    }
}
