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

    void HandleWorkerPurchase(double price, int power)
    {
        data.pointsPerSecond += power;

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("Buy Sound");

        double currentPoints = data.pointsCounterFloat;

        if (prefabs != null)
            prefabs.UpdateAllPrefabs(currentPoints, data.pointsPerSecond);

        if (stats != null)
            stats.UpdateAllStats(currentPoints, data.pointsPerSecond);
    }
}