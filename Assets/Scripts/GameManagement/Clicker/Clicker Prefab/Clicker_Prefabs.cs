using UnityEngine;

public class Clicker_Prefabs : MonoBehaviour
{
    [SerializeField] Prefab_Points prefabTotalPointsText;
    [SerializeField] Prefab_Timer prefabTimerText;

    public void UpdateAllPrefabs(int totalPoints)
    {
        if (prefabTotalPointsText != null)
        {
            prefabTotalPointsText.UpdateTotalPointsPrefab(totalPoints);
        }

        if (prefabTimerText != null)
        {
            prefabTimerText.UpdateTimerPrefab();
        }
    }
}
