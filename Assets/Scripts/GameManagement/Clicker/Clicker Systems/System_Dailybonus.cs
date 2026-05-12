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
            ApplyBonus(1);
            return;
        }

        DateTime lastDate = DateTime.Parse(data.lastBonusDate);
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
        data.lastBonusDate = DateTime.Today.ToString();

        float baseMultiplier = 1.0f + (newStreak - 1) * 0.1f;
        float milestoneBonus = 0f;

        if (newStreak >= 365) milestoneBonus = 25.0f;
        else if (newStreak >= 180) milestoneBonus = 10.0f;
        else if (newStreak >= 90) milestoneBonus = 5.0f;
        else if (newStreak >= 60) milestoneBonus = 3.0f;
        else if (newStreak >= 30) milestoneBonus = 2.0f;
        else if (newStreak >= 21) milestoneBonus = 1.0f;
        else if (newStreak >= 14) milestoneBonus = 0.5f;
        else if (newStreak >= 7) milestoneBonus = 0.2f;

        data.currentDailyMultiplier = baseMultiplier + milestoneBonus;

        UpdateUI();
        Debug.Log($"Daily Bonus: Dzień {newStreak}. Mnożnik: {data.currentDailyMultiplier:F1}x");
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
        int[] milestones = { 7, 14, 21, 30, 60, 90, 180, 365 };
        foreach (int m in milestones)
        {
            if (currentStreak < m) return m;
        }
        return 0;
    }
}