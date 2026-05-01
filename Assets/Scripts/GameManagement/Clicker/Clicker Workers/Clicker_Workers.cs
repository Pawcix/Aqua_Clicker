using UnityEngine;

public class Clicker_Workers : MonoBehaviour
{
    [SerializeField] System_Data data;
    [SerializeField] Clicker_Prefabs prefabs;
    [SerializeField] Clicker_Stats stats;

    void OnEnable()
    {
        Clicker_System.OnItemBought.AddListener(HandleWorkerPurchase);
    }

    void OnDisable()
    {
        Clicker_System.OnItemBought.RemoveListener(HandleWorkerPurchase);
    }

    void HandleWorkerPurchase(int price, int power)
    {
        data.pointsPerSecond += power;

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("Buy Sound");

        int currentPointsInt = Mathf.RoundToInt(data.pointsCounterFloat);

        if (prefabs != null)
            prefabs.UpdateAllPrefabs(currentPointsInt, data.pointsPerSecond);

        if (stats != null)
            stats.UpdateAllStats(currentPointsInt, data.pointsPerSecond);
    }
}