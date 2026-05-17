using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class RiskReward : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] System_Data data;
    [SerializeField] GameObject boosterIconPrefab;

    [Header("Dynamic Icons (Visuals):")]
    [SerializeField] Sprite winIconSprite;
    [SerializeField] Sprite loseIconSprite;

    GameObject activeIconInstance;
    TextMeshProUGUI timerTextInIcon;
    Image iconDisplayInIcon;

    private const string RiskEndTimeKey = "RiskBonusEndTime";

    void Start()
    {
        ValidateBonusOnLines();
    }

    void Update()
    {
        if (data.riskMultiplier != 1.0f)
        {
            if (!PlayerPrefs.HasKey(RiskEndTimeKey))
            {
                DateTime endTime = DateTime.Now.AddSeconds(data.riskBonusTimer);
                PlayerPrefs.SetString(RiskEndTimeKey, endTime.ToString());
                PlayerPrefs.Save();
            }

            DateTime bonusEndTime = DateTime.Parse(PlayerPrefs.GetString(RiskEndTimeKey));
            TimeSpan timeRemaining = bonusEndTime - DateTime.Now;

            if (timeRemaining.TotalSeconds > 0)
            {
                data.riskBonusTimer = (float)timeRemaining.TotalSeconds;

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

    void ValidateBonusOnLines()
    {
        if (PlayerPrefs.HasKey(RiskEndTimeKey))
        {
            DateTime bonusEndTime = DateTime.Parse(PlayerPrefs.GetString(RiskEndTimeKey));

            if (DateTime.Now >= bonusEndTime)
            {
                PlayerPrefs.DeleteKey(RiskEndTimeKey);
                PlayerPrefs.Save();
                if (data != null)
                {
                    data.riskMultiplier = 1.0f;
                    data.riskBonusTimer = 0;
                }
            }
            else
            {
                if (data != null)
                {
                    data.riskBonusTimer = (float)(bonusEndTime - DateTime.Now).TotalSeconds;
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

            Transform iconTransform = activeIconInstance.transform.Find("Img - Risk Reward");
            if (iconTransform != null)
            {
                iconDisplayInIcon = iconTransform.GetComponent<Image>();

                if (iconDisplayInIcon != null)
                {
                    if (data.riskMultiplier > 1.0f)
                    {
                        iconDisplayInIcon.sprite = winIconSprite;
                    }
                    else
                    {
                        iconDisplayInIcon.sprite = loseIconSprite;
                    }
                }
            }
        }
    }

    void UpdateIconUI(TimeSpan timeRemaining)
    {
        if (timerTextInIcon == null) return;

        int totalSeconds = Mathf.CeilToInt((float)timeRemaining.TotalSeconds);

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

        if (PlayerPrefs.HasKey(RiskEndTimeKey))
        {
            PlayerPrefs.DeleteKey(RiskEndTimeKey);
            PlayerPrefs.Save();
        }

        if (data != null)
        {
            data.riskMultiplier = 1.0f;
            data.riskBonusTimer = 0;
        }
    }
}