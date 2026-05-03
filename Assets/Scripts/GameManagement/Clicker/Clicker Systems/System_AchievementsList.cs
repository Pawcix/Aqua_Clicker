using UnityEngine;
using System.Collections.Generic;

public class System_AchievementsList : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] System_Data data;
    [SerializeField] System_Achievements achievementManager;

    [Header("UI Settings:")]
    [SerializeField] GameObject achievementPrefab;
    [SerializeField] Transform container;

    void OnEnable() => RefreshList();

    void ClearContainer()
    {
        foreach (Transform child in container) Destroy(child.gameObject);
    }

    public void RefreshList()
    {
        if (container == null || achievementManager == null || data == null) return;

        if (!achievementManager.IsReady())
        {
            ClearContainer();
            return;
        }

        ClearContainer();
        List<Achievement> allAchievements = achievementManager.GetAllAchievements();

        foreach (Achievement ach in allAchievements)
        {
            if (ach == null) continue;
            GameObject go = Instantiate(achievementPrefab, container);
            go.transform.localScale = Vector3.one;

            Prefab_Achievements prefabScript = go.GetComponent<Prefab_Achievements>();
            if (prefabScript != null)
            {
                bool isUnlocked = data.unlockedAchievementIDs.Contains(ach.id);
                prefabScript.Setup(ach, isUnlocked);
            }
        }
    }
}