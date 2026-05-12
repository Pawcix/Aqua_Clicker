using UnityEngine;
using System;

public class System_NotificationDailyBonus : MonoBehaviour
{
    [SerializeField] System_Data data;
    public static System_NotificationDailyBonus Instance;
    public GameObject badgeObject;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        CheckDailyNotification();
    }

    public void CheckDailyNotification()
    {
        if (data == null || badgeObject == null) return;

        bool isReady = data.lastBonusDate != DateTime.Today.ToString();

        badgeObject.SetActive(isReady);
    }

    public void SetAlert(bool state)
    {
        if (badgeObject != null)
            badgeObject.SetActive(state);
    }
}