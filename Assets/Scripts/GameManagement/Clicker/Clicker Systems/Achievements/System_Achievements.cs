using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

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

    [Header("Animation Settings:")]
    [SerializeField] private float animationSpeed = 4f;
    [SerializeField] private float displayDuration = 5f;

    private RectTransform notificationRect;
    private bool isSystemReady = false;

    void Awake()
    {
        if (Instance == null) Instance = this;

        if (notificationPanel != null)
        {
            notificationRect = notificationPanel.GetComponent<RectTransform>();
        }
    }

    void Start()
    {
        if (notificationPanel != null && notificationRect != null)
        {
            notificationPanel.SetActive(true);

            LayoutRebuilder.ForceRebuildLayoutImmediate(notificationRect);

            float startHiddenX = -(notificationRect.sizeDelta.x + 50f);

            notificationRect.anchoredPosition = new Vector2(startHiddenX, notificationRect.anchoredPosition.y);
        }

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

        if (System_WardrobeUnlockSkin.Instance != null)
        {
            System_WardrobeUnlockSkin.Instance.GrantReward(ach);
        }

        if (notificationPanel != null && notificationRect != null)
        {
            StopAllCoroutines();

            if (notificationText != null)
            {
                notificationText.text = "<color=#FFD700>ACHIEVEMENT UNLOCKED:</color>\n" + ach.title;
            }

            if (notificationIcon != null) notificationIcon.sprite = ach.icon;

            StartCoroutine(ShowNotificationRoutine());
        }

        System_AchievementsList listUI = Object.FindFirstObjectByType<System_AchievementsList>();
        if (listUI != null) listUI.RefreshList();

        if (Data_SaveManager.instance != null) Data_SaveManager.instance.SaveGame();
    }

    IEnumerator ShowNotificationRoutine()
    {
        float timer = 0f;
        float currentY = notificationRect.anchoredPosition.y;

        LayoutRebuilder.ForceRebuildLayoutImmediate(notificationRect);

        float width = notificationRect.sizeDelta.x;
        float dynamicHiddenX = -(width + 50f);

        while (timer < 1f)
        {
            timer += Time.unscaledDeltaTime * animationSpeed;
            float smoothStep = Mathf.SmoothStep(0f, 1f, timer);
            float newX = Mathf.Lerp(dynamicHiddenX, 0f, smoothStep); 

            notificationRect.anchoredPosition = new Vector2(newX, currentY);
            yield return null;
        }
        notificationRect.anchoredPosition = new Vector2(0f, currentY);

        yield return new WaitForSecondsRealtime(displayDuration);

        timer = 0f;
        while (timer < 1f)
        {
            timer += Time.unscaledDeltaTime * animationSpeed;
            float smoothStep = Mathf.SmoothStep(0f, 1f, timer);
            float newX = Mathf.Lerp(0f, dynamicHiddenX, smoothStep);

            notificationRect.anchoredPosition = new Vector2(newX, currentY);
            yield return null;
        }
        notificationRect.anchoredPosition = new Vector2(dynamicHiddenX, currentY);
    }

    public List<Achievement> GetAllAchievements() => allAchievements;
}