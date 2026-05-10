using TMPro;
using System;
using UnityEngine;

public class System_AwayIncome : MonoBehaviour
{
    [SerializeField] System_Data data;
    [SerializeField] Clicker_Stats stats;
    [SerializeField] Clicker_Prefabs prefabs;
    [SerializeField] GameObject awayIncomeDisplayBox;
    [SerializeField] TextMeshProUGUI awayIncomeNotificationText;

    void Start()
    {
        Invoke(nameof(CalculateAwayIncome), 0.5f);
    }

    void OnApplicationQuit() { UpdateLastSeen(); }
    void OnApplicationPause(bool pause) { if (pause) UpdateLastSeen(); }

    public void CalculateAwayIncome()
    {
        if (!PlayerPrefs.HasKey("LastSeen"))
        {
            UpdateLastSeen();
            return;
        }

        string lastSeenStr = PlayerPrefs.GetString("LastSeen");

        if (DateTime.TryParse(lastSeenStr, out DateTime lastSeen))
        {
            DateTime now = DateTime.Now;
            TimeSpan timeAway = now - lastSeen;
            double secondsAway = timeAway.TotalSeconds;

            if (secondsAway < 10) return;

            double maxSeconds = 2592000;
            if (secondsAway > maxSeconds) secondsAway = maxSeconds;

            double totalBasePPS = data.basePPS + data.workersPPS;

            if (totalBasePPS > 0)
            {

                float masteryBonus = 0f;
                if (Mastery.Instance != null)
                {
                    masteryBonus = Mastery.Instance.GetMasteryBonus(Mastery.MasteryType.AwayIncome);
                }

                double earned = (totalBasePPS * secondsAway) * (1.0f + masteryBonus);

                if (earned > 0)
                {
                    data.pointsCounterFloat += earned;
                    data.totalAwayEarnings += earned;

                    if (Mastery.Instance != null)
                    {
                        float xpGained = (float)(secondsAway / 60f);
                        Mastery.Instance.AddMasteryXP(Mastery.MasteryType.AwayIncome, xpGained);
                    }

                    if (System_Leveling.Instance != null)
                    {
                        System_Leveling.Instance.RegisterPointGain(earned);
                    }

                    DisplayNotification(earned, timeAway);
                    UpdateAllUI();
                }
            }
        }

        UpdateLastSeen();
    }

    public void UpdateLastSeen()
    {
        PlayerPrefs.SetString("LastSeen", DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    void DisplayNotification(double earned, TimeSpan time)
    {
        if (awayIncomeNotificationText != null && awayIncomeDisplayBox != null)
        {
            string timeStr = string.Format("{0:D2}:{1:D2}:{2:D2}",
                (int)time.TotalHours, time.Minutes, time.Seconds);

            awayIncomeNotificationText.text = $"Earned: <color=green>+{NumberFormatter.FormatWithDots(earned)}</color>\n<size=70%>Time Away: {timeStr}</size>";

            awayIncomeDisplayBox.SetActive(true);

            CancelInvoke(nameof(HideNotification));
            Invoke(nameof(HideNotification), 10f);
        }
    }

    void HideNotification()
    {
        if (awayIncomeDisplayBox != null)
            awayIncomeDisplayBox.SetActive(false);
    }

    void UpdateAllUI()
    {
        if (data == null) return;

        double currentTotal = System.Math.Floor(data.pointsCounterFloat);
        double activePPS = (data.basePPS + data.workersPPS) * data.currentDailyMultiplier;

        if (stats != null) stats.UpdateAllStats(currentTotal, (float)activePPS);
        if (prefabs != null) prefabs.UpdateAllPrefabs(currentTotal, activePPS);
    }
}
