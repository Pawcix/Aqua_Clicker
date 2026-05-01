using UnityEngine;

public class System_PassiveProduction : MonoBehaviour
{
    [SerializeField] System_Data data;
    [SerializeField] Clicker_Prefabs prefabs;
    [SerializeField] Clicker_Stats stats;

    float timer = 0f;

    void Update()
    {
        if (data.pointsPerSecond <= 0) return;

        data.pointsCounterFloat += data.pointsPerSecond * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            RefreshHeavyUI();
            timer = 0f;
        }
    }

    void RefreshHeavyUI()
    {
        int currentPointsInt = (int)data.pointsCounterFloat;

        if (prefabs != null)
            prefabs.UpdateAllPrefabs(currentPointsInt, data.pointsPerSecond);

        if (stats != null)
            stats.UpdateAllStats(currentPointsInt, data.pointsPerSecond);
    }
}
