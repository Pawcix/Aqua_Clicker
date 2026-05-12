using UnityEngine;
using System;

public class System_NotificationWheelReward : MonoBehaviour
{
    public static System_NotificationWheelReward Instance;
    [SerializeField] GameObject badgeObject;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Update()
    {
        CheckWheelNotification();
    }

    public void CheckWheelNotification()
    {
        if (badgeObject == null) return;
        if (!PlayerPrefs.HasKey("NextWheelSpin"))
        {
            SetAlert(true);
            return;
        }

        DateTime nextSpinTime = DateTime.Parse(PlayerPrefs.GetString("NextWheelSpin"));

        bool isReady = DateTime.Now >= nextSpinTime;

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