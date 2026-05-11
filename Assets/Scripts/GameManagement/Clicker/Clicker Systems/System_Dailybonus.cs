using TMPro;
using System;
using UnityEngine;

public class System_Dailybonus : MonoBehaviour
{
    [SerializeField] System_Data data;
    [SerializeField] TextMeshProUGUI streakText;
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
            data.lastBonusDate = DateTime.Today.ToString();
            data.loginStreak = 1;
            data.currentDailyMultiplier = 1.1f;
            UpdateUI();
            return;
        }

        DateTime lastDate = DateTime.Parse(data.lastBonusDate);
        DateTime today = DateTime.Today;
        TimeSpan difference = today - lastDate;

        double totalDays = difference.TotalDays;

        if (totalDays >= 1 && totalDays < 2)
        {
            ApplyBonus(data.loginStreak + 1);
        }
        else if (totalDays >= 2)
        {
            ApplyBonus(1);
        }
        else
        {
            UpdateUI();
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
            CheckDailyBonus();
        }
    }

    void ApplyBonus(int newStreak)
    {
        data.loginStreak = newStreak;
        data.lastBonusDate = DateTime.Today.ToString();

        float baseMultiplier = 1.0f + (newStreak * 0.1f);
        float milestoneBonus = 0f;

        if (newStreak >= 7) milestoneBonus += 0.5f;
        if (newStreak >= 14) milestoneBonus += 0.5f;
        if (newStreak >= 21) milestoneBonus += 1.0f;
        if (newStreak >= 30) milestoneBonus += 2.0f;
        if (newStreak >= 60) milestoneBonus += 3.0f;
        if (newStreak >= 90) milestoneBonus += 5.0f;
        if (newStreak >= 180) milestoneBonus += 10.0f;
        if (newStreak >= 365) milestoneBonus += 25.0f;

        data.currentDailyMultiplier = baseMultiplier + milestoneBonus;

        UpdateUI();
        Debug.Log($"Daily Bonus przyznany! Dzień: {newStreak}");
    }

    void UpdateUI()
    {
        if (streakText != null)
            streakText.text = $"STREAK: {data.loginStreak} DAYS";

        if (bonusText != null)
            bonusText.text = $"TOTAL MULTIPLIER: {data.currentDailyMultiplier:F1}x";

        if (nextMilestoneText != null)
        {
            int nextGoal = GetNextMilestone(data.loginStreak);
            if (nextGoal > 0)
                nextMilestoneText.text = $"Next Milestone in: {nextGoal - data.loginStreak} days";
            else
                nextMilestoneText.text = "YOU ARE A LEGEND!";
        }
    }

    int GetNextMilestone(int currentStreak)
    {
        if (currentStreak < 7) return 7;
        if (currentStreak < 14) return 14;
        if (currentStreak < 21) return 21;
        if (currentStreak < 30) return 30;
        if (currentStreak < 60) return 60;
        if (currentStreak < 90) return 90;
        if (currentStreak < 180) return 180;
        if (currentStreak < 365) return 365;
        return 0;
    }
}
