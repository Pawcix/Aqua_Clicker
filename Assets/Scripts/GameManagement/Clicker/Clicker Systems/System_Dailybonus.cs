using TMPro;
using System;
using UnityEngine;

public class System_Dailybonus : MonoBehaviour
{
    [SerializeField] System_Data data;
    [SerializeField] TextMeshProUGUI bonusText;
    [SerializeField] TextMeshProUGUI nextMilestoneText;
    [SerializeField] TextMeshProUGUI timerText;

    void Start()
    {
        CheckDailyBonus();
    }

    void Update()
    {
        UpdateTimerUI();
    }

    public void CheckDailyBonus()
    {
        if (string.IsNullOrEmpty(data.lastBonusDate))
        {
            ApplyBonus(1);
            return;
        }

        if (DateTime.TryParse(data.lastBonusDate, out DateTime lastDate))
        {
            DateTime today = DateTime.Today;
            TimeSpan difference = today - lastDate;
            int daysPassed = (int)difference.TotalDays;

            if (daysPassed == 1)
            {
                ApplyBonus(data.loginStreak + 1);
            }
            else if (daysPassed > 1)
            {
                ApplyBonus(1);
            }
            else
            {
                UpdateUI();
            }
        }
        else
        {
            ApplyBonus(1);
        }
    }

    void UpdateTimerUI()
    {
        if (timerText == null) return;

        DateTime nextDay = DateTime.Today.AddDays(1);
        TimeSpan timeRemaining = nextDay - DateTime.Now;

        if (timeRemaining.TotalSeconds > 0)
        {
            timerText.text = string.Format("NEXT BONUS IN: {0:D2}:{1:D2}:{2:D2}",
                timeRemaining.Hours,
                timeRemaining.Minutes,
                timeRemaining.Seconds);
        }
        else
        {
            timerText.text = "BONUS READY!";
        }
    }

    void ApplyBonus(int newStreak)
    {
        data.loginStreak = newStreak;
        data.lastBonusDate = DateTime.Today.ToString("yyyy-MM-dd");

        float baseMultiplier = 1.0f;
        if (newStreak > 1)
        {
            baseMultiplier += (newStreak - 1) * 0.1f;
        }

        data.currentDailyMultiplier = baseMultiplier;
        UpdateUI();
    }

    void UpdateUI()
    {
        bool isFirstDay = data.loginStreak <= 1;

        if (bonusText != null)
        {
            if (isFirstDay)
            {
                bonusText.text = "<color=#FFA500>STREAK STARTED!</color>\n<size=75%>Come back tomorrow for your first bonus!</size>";
            }
            else
            {
                bonusText.text = $"TOTAL MULTIPLIER:\n<size=160%><color=#00FF00><b>{data.currentDailyMultiplier:F1}x</b></color></size>";
            }
        }

        if (nextMilestoneText != null)
        {
            if (isFirstDay)
            {
                nextMilestoneText.gameObject.SetActive(false);
            }
            else
            {
                nextMilestoneText.gameObject.SetActive(true);

                string dayWord = data.loginStreak == 1 ? "DAY" : "DAYS";
                nextMilestoneText.text = $"CURRENT STREAK:\n<size=160%><color=#FFD700><b>{data.loginStreak} {dayWord} STREAK</b></color></size>";
            }
        }
    }
}