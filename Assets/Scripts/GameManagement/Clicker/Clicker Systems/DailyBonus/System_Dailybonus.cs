using UnityEngine;
using TMPro;
using System;

public class System_DailyBonus : MonoBehaviour
{
    [SerializeField] System_Data data;
    [SerializeField] TextMeshProUGUI totalMultiplierText;
    [SerializeField] TextMeshProUGUI timerText;

    [Header("List configuration")]
    [SerializeField] Transform numbersContainer;
    [SerializeField] TextMeshProUGUI numberPrefab;

    void Start()
    {
        CheckDailyBonus();
        UpdateUI();
        RefreshNumbersDisplay();
    }

    void Update() => UpdateTimerUI();

    public void CheckDailyBonus()
    {
        DateTime today = DateTime.Today;
        if (string.IsNullOrEmpty(data.lastBonusDate) || !DateTime.TryParse(data.lastBonusDate, out DateTime lastDate))
        {
            ApplyBonus(1);
            return;
        }

        int daysPassed = (today - lastDate).Days;
        if (daysPassed == 1) ApplyBonus(data.loginStreak + 1);
        else if (daysPassed > 1) ApplyBonus(1);
    }

    void ApplyBonus(int newStreak)
    {
        data.loginStreak = newStreak;
        data.lastBonusDate = DateTime.Today.ToString("yyyy-MM-dd");
        data.currentDailyMultiplier = 1.0f + ((newStreak - 1) * 0.05f);

        UpdateUI();
        RefreshNumbersDisplay();
    }

    void RefreshNumbersDisplay()
    {
        foreach (Transform child in numbersContainer) Destroy(child.gameObject);

        for (int i = -2; i < 0; i++)
        {
            int dayNumber = data.loginStreak + i;
            if (dayNumber < 1)
            {
                GameObject empty = new GameObject("EmptySlot");
                empty.transform.SetParent(numbersContainer);
                empty.AddComponent<UnityEngine.UI.LayoutElement>().minWidth = 180f;
            }
        }

        for (int i = -2; i <= 2; i++)
        {
            int dayNumber = data.loginStreak + i;
            if (dayNumber < 1) continue;

            TextMeshProUGUI numObj = Instantiate(numberPrefab, numbersContainer);
            numObj.text = dayNumber.ToString();

            float arcHeight = 60f;
            float yPos = Mathf.Sin(Mathf.Abs(i) * 0.8f) * -arcHeight;
            numObj.rectTransform.anchoredPosition = new Vector2(numObj.rectTransform.anchoredPosition.x, yPos);

            if (i == 0)
            {
                numObj.fontSize = 225;
                numObj.color = new Color(0, 1, 0, 1f);
            }
            else
            {
                numObj.fontSize = 150 - (Mathf.Abs(i) * 40);
                numObj.color = new Color(1, 1, 1, 1f - (Mathf.Abs(i) * 0.3f));
            }
        }
    }

    void UpdateTimerUI()
    {
        TimeSpan remaining = DateTime.Today.AddDays(1) - DateTime.Now;
        timerText.text = remaining.TotalSeconds > 0
            ? $"NEXT BONUS: {remaining:hh\\:mm\\:ss}"
            : "BONUS READY!";
    }

    void UpdateUI()
    {
        totalMultiplierText.text = $"TOTAL MULTIPLIER\n<color=green>{data.currentDailyMultiplier:F2}x</color>";
    }
}