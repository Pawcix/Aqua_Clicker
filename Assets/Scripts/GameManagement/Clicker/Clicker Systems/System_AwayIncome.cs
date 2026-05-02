using TMPro;
using System;
using UnityEngine;

public class System_AwayIncome : MonoBehaviour
{
    [SerializeField] System_Data data;
    [SerializeField] Clicker_Stats stats;
    [SerializeField] Clicker_Prefabs prefabs;
    [SerializeField] TextMeshProUGUI awayIncomeNotificationText;

    void Start()
    {
        if (awayIncomeNotificationText != null)
            awayIncomeNotificationText.gameObject.SetActive(false);

        Invoke(nameof(CalculateAwayIncome), 0.5f);
    }

    public void CalculateAwayIncome()
    {
        if (!PlayerPrefs.HasKey("LastSeen")) return;

        string lastSeenStr = PlayerPrefs.GetString("LastSeen");

        if (DateTime.TryParse(lastSeenStr, out DateTime lastSeen))
        {
            TimeSpan timeAway = DateTime.Now - lastSeen;
            double secondsAway = timeAway.TotalSeconds;

            if (secondsAway > 10 && data.pointsPerSecond > 0)
            {
                double earned = secondsAway * (double)data.pointsPerSecond;

                if (earned > 0)
                {
                    data.pointsCounterFloat += earned;
                    data.totalAwayEarnings += earned;

                    DisplayNotification(earned, timeAway);
                    UpdateAllUI();
                }
            }
        }
    }

    void DisplayNotification(double earned, TimeSpan time)
    {
        if (awayIncomeNotificationText != null)
        {
            awayIncomeNotificationText.gameObject.SetActive(true);

            string timeStr = string.Format("{0:D2}:{1:D2}:{2:D2}",
                (int)time.TotalHours, time.Minutes, time.Seconds);

            awayIncomeNotificationText.text = $"Recently Earned: <color=green>+{NumberFormatter.FormatWithDots(earned)}</color>\n<size=70%>Time Away: {timeStr}</size>";

            Invoke(nameof(HideNotification), 10f);
        }
    }

    void HideNotification()
    {
        if (awayIncomeNotificationText != null)
            awayIncomeNotificationText.gameObject.SetActive(false);
    }

    void UpdateAllUI()
    {
        if (data == null) return;

        double currentPoints = data.pointsCounterFloat;

        if (stats != null) stats.UpdateAllStats(currentPoints, data.pointsPerSecond);
        if (prefabs != null) prefabs.UpdateAllPrefabs(currentPoints, data.pointsPerSecond);
    }
}
