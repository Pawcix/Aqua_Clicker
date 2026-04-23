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
        if (data.pointsCounter >= price)
        {
            data.pointsCounter -= price;
            data.pointsPerSecond += power;

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX("Buy Sound");

            if (prefabs != null) prefabs.UpdateAllPrefabs(data.pointsCounter, data.pointsPerSecond);
            if (stats != null) stats.UpdateAllStats(data.pointsCounter, data.pointsPerSecond);
        }
    }
}