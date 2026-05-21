using TMPro;
using UnityEngine;

public class System_AchievementsProgressBar : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] System_Data data;
    [SerializeField] System_Achievements achievementManager;

    [Header("UI Elements:")]
    [SerializeField] TextMeshProUGUI progressText;

    public void UpdateProgressBar()
    {
        if (data == null || achievementManager == null) return;

        var allAchievements = achievementManager.GetAllAchievements();
        if (allAchievements == null || allAchievements.Count == 0) return;

        int totalCount = allAchievements.Count;
        int unlockedCount = data.unlockedAchievementIDs.Count;

        float progress01 = (float)unlockedCount / totalCount;
        float percentage = progress01 * 100f;

        if (progressText != null)
        {
            progressText.text = $"Achievements:\n{unlockedCount} / {totalCount} ({percentage:F0}%)";
        }
    }
}