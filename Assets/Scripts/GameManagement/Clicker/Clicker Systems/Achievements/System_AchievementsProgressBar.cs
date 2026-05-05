using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class System_AchievementsProgressBar : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] System_Data data;
    [SerializeField] System_Achievements achievementManager;

    [Header("UI Elements:")]
    [SerializeField] Slider progressSlider;
    [SerializeField] TextMeshProUGUI progressText;

    public void UpdateProgressBar()
    {
        if (data == null || achievementManager == null || progressSlider == null) return;

        var allAchievements = achievementManager.GetAllAchievements();
        if (allAchievements == null || allAchievements.Count == 0) return;

        int totalCount = allAchievements.Count;
        int unlockedCount = data.unlockedAchievementIDs.Count;

        float progress01 = (float)unlockedCount / totalCount;
        float percentage = progress01 * 100f;

        progressSlider.value = progress01;

        if (progressText != null)
        {
            progressText.text = $"Achievements Progress: {unlockedCount} / {totalCount} ({percentage:F0}%)";
        }
    }
}
