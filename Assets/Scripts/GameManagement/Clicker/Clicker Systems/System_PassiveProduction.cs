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
        double currentPoints = data.pointsCounterFloat;

        if (prefabs != null)
            prefabs.UpdateAllPrefabs(currentPoints, data.pointsPerSecond);

        if (stats != null)
            stats.UpdateAllStats(currentPoints, data.pointsPerSecond);
    }
}
