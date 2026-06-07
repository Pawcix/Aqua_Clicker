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

    const string WheelNextSpinKey = "NextWheelSpin";

    void Start()
    {
        ValidateBonusOnStart();
    }

    void Update()
    {
        if (data.wheelMultiplier != 1.0f && PlayerPrefs.HasKey(WheelNextSpinKey))
        {
            DateTime nextSpinTime = DateTime.Parse(PlayerPrefs.GetString(WheelNextSpinKey));
            TimeSpan timeRemaining = nextSpinTime - DateTime.Now;

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
        if (PlayerPrefs.HasKey(WheelNextSpinKey))
        {
            DateTime nextSpinTime = DateTime.Parse(PlayerPrefs.GetString(WheelNextSpinKey));

            if (DateTime.Now >= nextSpinTime)
            {
                ResetBonus();
            }
            else
            {
                if (data != null)
                {
                    data.wheelBonusTimer = (float)(nextSpinTime - DateTime.Now).TotalSeconds;
                }
            }
        }
        else
        {
            ResetBonus();
        }
    }

    void SpawnBoosterIcon()
    {
        activeIconInstance = Event_Manager.Instance.AddEventIcon(boosterIconPrefab);

        if (activeIconInstance != null)
        {
            activeIconInstance.transform.SetAsFirstSibling();
            timerTextInIcon = activeIconInstance.GetComponentInChildren<TextMeshProUGUI>();

            Image[] allImages = activeIconInstance.GetComponentsInChildren<Image>();
            foreach (Image img in allImages)
            {
                if (img.gameObject != activeIconInstance)
                {
                    iconDisplayInIcon = img;
                    break;
                }
            }

            if (iconDisplayInIcon != null && data.currentWheelRewardIcon != null)
            {
                iconDisplayInIcon.sprite = data.currentWheelRewardIcon;
                iconDisplayInIcon.color = Color.white;
            }
        }
    }

    void UpdateIconUI(TimeSpan timeRemaining)
    {
        if (timerTextInIcon == null) return;

        int secs = timeRemaining.Seconds;
        int mins = (int)timeRemaining.TotalMinutes;

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

        ResetBonus();
    }

    void ResetBonus()
    {
        if (data != null)
        {
            data.wheelMultiplier = 1.0f;
            data.wheelBonusTimer = 0;
            data.currentWheelRewardIcon = null;
        }
    }
}