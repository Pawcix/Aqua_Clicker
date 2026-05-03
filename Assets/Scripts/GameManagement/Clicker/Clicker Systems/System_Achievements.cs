using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class System_Achievements : MonoBehaviour
{
    public static System_Achievements Instance;

    [Header("References:")]
    [SerializeField] System_Data data;
    [SerializeField] List<Achievement> allAchievements;

    [Header("Notification UI:")]
    [SerializeField] GameObject notificationPanel;
    [SerializeField] TextMeshProUGUI notificationText;
    [SerializeField] Image notificationIcon;

    private bool isSystemReady = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        if (notificationPanel != null) notificationPanel.SetActive(false);
        Invoke(nameof(EnableChecking), 1.0f);
    }

    public void EnableChecking()
    {
        if (isSystemReady) return;
        isSystemReady = true;

        System_AchievementsList listUI = Object.FindFirstObjectByType<System_AchievementsList>();
        if (listUI != null) listUI.RefreshList();
    }

    public void DisableChecking() => isSystemReady = false;
    public bool IsReady() => isSystemReady;

    public void CheckAchievements()
    {
        if (!isSystemReady || data == null || data.unlockedAchievementIDs == null) return;

        foreach (var ach in allAchievements)
        {
            if (ach == null) continue;
            if (data.unlockedAchievementIDs.Contains(ach.id)) continue;

            bool isConditionMet = false;
            switch (ach.type)
            {
                case AchievementType.TotalPoints:
                    if (data.pointsCounterFloat > 0)
                        isConditionMet = data.pointsCounterFloat >= ach.requiredValue;
                    break;
                case AchievementType.GoldenDrops:
                    if (data.goldenDrops > 0)
                        isConditionMet = data.goldenDrops >= (int)ach.requiredValue;
                    break;
            }

            if (isConditionMet) UnlockAchievement(ach);
        }
    }

    void UnlockAchievement(Achievement ach)
    {
        if (data.unlockedAchievementIDs.Contains(ach.id)) return;
        data.unlockedAchievementIDs.Add(ach.id);

        if (notificationPanel != null)
        {
            StopAllCoroutines();
            if (notificationText != null) notificationText.text = ach.title;
            if (notificationIcon != null) notificationIcon.sprite = ach.icon;
            StartCoroutine(ShowNotificationRoutine());
        }

        System_AchievementsList listUI = Object.FindFirstObjectByType<System_AchievementsList>();
        if (listUI != null) listUI.RefreshList();

        if (Data_SaveManager.instance != null) Data_SaveManager.instance.SaveGame();
    }

    System.Collections.IEnumerator ShowNotificationRoutine()
    {
        notificationPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        notificationPanel.SetActive(false);
    }

    public List<Achievement> GetAllAchievements() => allAchievements;
}