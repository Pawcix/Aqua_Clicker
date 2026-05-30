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
    [SerializeField] float animationSpeed = 4f;
    [SerializeField] float displayDuration = 5f;

    RectTransform notificationRect;
    bool isSystemReady = false;

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

            float startHiddenY = -(notificationRect.sizeDelta.y + 100f);

            notificationRect.anchoredPosition = new Vector2(0f, startHiddenY);
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

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("Achievement");

        float timer = 0f;
        LayoutRebuilder.ForceRebuildLayoutImmediate(notificationRect);

        float height = notificationRect.sizeDelta.y;
        float dynamicHiddenY = -(height + 100f);
        float targetVisibleY = 25f;

        while (timer < 1f)
        {
            timer += Time.unscaledDeltaTime * animationSpeed;
            float smoothStep = Mathf.SmoothStep(0f, 1f, timer);
            float newY = Mathf.Lerp(dynamicHiddenY, targetVisibleY, smoothStep);

            notificationRect.anchoredPosition = new Vector2(0f, newY);
            yield return null;
        }
        notificationRect.anchoredPosition = new Vector2(0f, targetVisibleY);

        yield return new WaitForSecondsRealtime(displayDuration);

        timer = 0f;
        while (timer < 1f)
        {
            timer += Time.unscaledDeltaTime * animationSpeed;
            float smoothStep = Mathf.SmoothStep(0f, 1f, timer);
            float newY = Mathf.Lerp(targetVisibleY, dynamicHiddenY, smoothStep);

            notificationRect.anchoredPosition = new Vector2(0f, newY);
            yield return null;
        }
        notificationRect.anchoredPosition = new Vector2(0f, dynamicHiddenY);
    }

    public List<Achievement> GetAllAchievements() => allAchievements;
}