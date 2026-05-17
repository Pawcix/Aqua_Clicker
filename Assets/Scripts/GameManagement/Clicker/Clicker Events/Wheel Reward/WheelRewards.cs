using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class WheelRewards : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] System_Data data;
    [SerializeField] GameObject boosterIconPrefab;

    GameObject activeIconInstance;
    TextMeshProUGUI timerTextInIcon;
    Image iconDisplayInIcon;

    private const string WheelEndTimeKey = "WheelBonusEndTime";

    void Start()
    {
        ValidateBonusOnStart();
    }

    void Update()
    {
        if (data.wheelMultiplier != 1.0f)
        {
            if (!PlayerPrefs.HasKey(WheelEndTimeKey))
            {
                DateTime endTime = DateTime.Now.AddSeconds(data.wheelBonusTimer);
                PlayerPrefs.SetString(WheelEndTimeKey, endTime.ToString());
                PlayerPrefs.Save();
            }

            DateTime bonusEndTime = DateTime.Parse(PlayerPrefs.GetString(WheelEndTimeKey));
            TimeSpan timeRemaining = bonusEndTime - DateTime.Now;

            if (timeRemaining.TotalSeconds > 0)
            {
                data.wheelBonusTimer = (float)timeRemaining.TotalSeconds;

                if (activeIconInstance == null)
                {
                    SpawnBoosterIcon();
                }

                UpdateIconUI(timeRemaining);
            }
            else
            {
                RemoveBoosterIcon();
            }
        }
        else
        {
            if (activeIconInstance != null)
            {
                RemoveBoosterIcon();
            }
        }
    }

    void ValidateBonusOnStart()
    {
        if (PlayerPrefs.HasKey(WheelEndTimeKey))
        {
            DateTime bonusEndTime = DateTime.Parse(PlayerPrefs.GetString(WheelEndTimeKey));

            if (DateTime.Now >= bonusEndTime)
            {
                PlayerPrefs.DeleteKey(WheelEndTimeKey);
                PlayerPrefs.Save();
                if (data != null)
                {
                    data.wheelMultiplier = 1.0f;
                    data.wheelBonusTimer = 0;
                    data.currentWheelRewardIcon = null;
                }
            }
            else
            {
                if (data != null)
                {
                    data.wheelBonusTimer = (float)(bonusEndTime - DateTime.Now).TotalSeconds;
                }
            }
        }
    }

    void SpawnBoosterIcon()
    {
        activeIconInstance = Event_Manager.Instance.AddEventIcon(boosterIconPrefab);

        if (activeIconInstance != null)
        {
            activeIconInstance.transform.SetAsFirstSibling();
            timerTextInIcon = activeIconInstance.GetComponentInChildren<TextMeshProUGUI>();

            Transform iconTransform = activeIconInstance.transform.Find("Img - Wheel Reward");
            if (iconTransform != null)
            {
                iconDisplayInIcon = iconTransform.GetComponent<Image>();

                if (iconDisplayInIcon != null && data.currentWheelRewardIcon != null)
                {
                    iconDisplayInIcon.sprite = data.currentWheelRewardIcon;
                }
            }
        }
    }

    void UpdateIconUI(TimeSpan timeRemaining)
    {
        if (timerTextInIcon == null) return;

        int mins = timeRemaining.Minutes;
        int secs = timeRemaining.Seconds;

        if (timeRemaining.TotalSeconds < 0) { mins = 0; secs = 0; }

        timerTextInIcon.text = string.Format("{0:D2}:{1:D2}", mins, secs);
    }

    void RemoveBoosterIcon()
    {
        if (Event_Manager.Instance != null && activeIconInstance != null)
        {
            Event_Manager.Instance.RemoveEventIcon(activeIconInstance);
        }

        activeIconInstance = null;
        timerTextInIcon = null;
        iconDisplayInIcon = null;

        if (PlayerPrefs.HasKey(WheelEndTimeKey))
        {
            PlayerPrefs.DeleteKey(WheelEndTimeKey);
            PlayerPrefs.Save();
        }

        if (data != null)
        {
            data.wheelMultiplier = 1.0f;
            data.wheelBonusTimer = 0;
            data.currentWheelRewardIcon = null;
        }
    }
}