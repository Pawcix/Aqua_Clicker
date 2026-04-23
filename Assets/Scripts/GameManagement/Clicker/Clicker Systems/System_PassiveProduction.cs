using UnityEngine;

public class System_PassiveProduction : MonoBehaviour
{
    [SerializeField] System_Data data;
    [SerializeField] Clicker_Prefabs prefabs;
    [SerializeField] Clicker_Stats stats;

    private float timer = 0f;

    void Update()
    {
        if (data.pointsPerSecond <= 0) return;

        timer += Time.deltaTime;

        if (timer >= 1f)
        {
            AddPassivePoints();
            timer = 0f;
        }
    }

    void AddPassivePoints()
    {
        data.pointsCounter += data.pointsPerSecond;

        if (prefabs != null) prefabs.UpdateAllPrefabs(data.pointsCounter, data.pointsPerSecond);
        if (stats != null) stats.UpdateAllStats(data.pointsCounter, data.pointsPerSecond);
    }
}
