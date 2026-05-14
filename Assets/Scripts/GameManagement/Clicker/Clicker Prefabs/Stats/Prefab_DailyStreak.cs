using TMPro;
using UnityEngine;

public class Prefab_DailyStreak : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI streakText;

    public void UpdateDailyStreakPrefab(int currentStreak)
    {
        if (streakText == null) return;
        streakText.text = $"Days Streak\n{currentStreak} DAYS";
    }
}