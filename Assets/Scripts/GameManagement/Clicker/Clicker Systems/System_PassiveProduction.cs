using UnityEngine;

public class System_PassiveProduction : MonoBehaviour
{
    [SerializeField] System_Data data;
    [SerializeField] Clicker_Prefabs prefabs;
    [SerializeField] Clicker_Stats stats;

    float timer = 0f;

    void Update()
    {
        if (data == null || data.pointsPerSecond <= 0) return;

        data.pointsCounterFloat += (double)data.pointsPerSecond * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            RefreshHeavyUI();
            timer = 0f;
        }
    }

    void RefreshHeavyUI()
    {
        if (data == null) return;

        double currentPoints = data.pointsCounterFloat;

        int displayPPS = (int)System.Math.Floor(data.pointsPerSecond);

        if (prefabs != null)
            prefabs.UpdateAllPrefabs(currentPoints, displayPPS);

        if (stats != null)
            stats.UpdateAllStats(currentPoints, displayPPS);
    }
}
