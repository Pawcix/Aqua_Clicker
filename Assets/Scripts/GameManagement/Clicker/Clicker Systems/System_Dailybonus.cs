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

        float baseMultiplier = 1.0f;
        if (newStreak > 1)
        {
            baseMultiplier += (newStreak - 1) * 0.1f;
        }

        float milestoneBonus = CalculateMilestone(newStreak);
        data.currentDailyMultiplier = baseMultiplier + milestoneBonus;

        UpdateUI();
    }

    void UpdateUI()
    {
        bool isFirstDay = data.loginStreak <= 1;

        if (bonusText != null)
        {
            if (isFirstDay)
            {
                bonusText.text = "<color=#FFA500>STREAK STARTED!</color>\n<size=70%>Come back tomorrow for your first bonus!</size>";
            }
            else
            {
                bonusText.text = $"TOTAL MULTIPLIER: <color=#00FF00>{data.currentDailyMultiplier:F1}x</color>";
            }
        }

        if (nextMilestoneText != null)
        {
            if (isFirstDay)
            {
                nextMilestoneText.text = "";
            }
            else
            {
                int nextGoal = GetNextMilestone(data.loginStreak);
                if (nextGoal > 0)
                    nextMilestoneText.text = $"Next Milestone in: {nextGoal - data.loginStreak} days";
                else
                    nextMilestoneText.text = "YOU ARE A LEGEND!";
            }
        }
    }

    float CalculateMilestone(int streak)
    {
        if (streak >= 365) return 25.0f;
        if (streak >= 180) return 10.0f;
        if (streak >= 90) return 5.0f;
        if (streak >= 60) return 3.0f;
        if (streak >= 30) return 2.0f;
        if (streak >= 21) return 1.0f;
        if (streak >= 14) return 0.5f;
        if (streak >= 7) return 0.2f;
        return 0f;
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