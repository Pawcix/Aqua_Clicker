using UnityEngine;
using System;

public class System_NotificationRiskReward : MonoBehaviour
{
    public static System_NotificationRiskReward Instance;

    [Header("UI Reference:")]
    [SerializeField] GameObject badgeObject;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Update()
    {
        CheckRiskNotification();
    }

    public void CheckRiskNotification()
    {
        if (badgeObject == null) return;

        if (!PlayerPrefs.HasKey("NextRiskRewardPlay"))
        {
            SetAlert(true);
            return;
        }

        DateTime nextPlayTime = DateTime.Parse(PlayerPrefs.GetString("NextRiskRewardPlay"));

        bool isReady = DateTime.Now >= nextPlayTime;

        if (badgeObject.activeSelf != isReady)
        {
            badgeObject.SetActive(isReady);
        }
    }

    public void SetAlert(bool state)
    {
        if (badgeObject != null)
            badgeObject.SetActive(state);
    }
}