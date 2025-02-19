using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

public class ClickerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI counterText;
    [SerializeField] TextMeshProUGUI colectPoints;
    [SerializeField] TextMeshProUGUI colectPerSecond;
    [SerializeField] TextMeshProUGUI clickPerSecond;
    [SerializeField] TextMeshProUGUI counterClick;
    [SerializeField] TextMeshProUGUI amountPurchase;
    [SerializeField] TextMeshProUGUI timerGame;

    float counterTimeStart = 0;

    void Start()
    {
        timerGame.text = counterTimeStart.ToString();
    }

    void Update()
    {
        CounterTimeInGame();
    }

    public void UpdateUI(int amount)
    {
        string formattedAmount = FormatNumberWithDots(amount);

        if (amount > 0)
        {
            counterText.text = $"Waters \n {formattedAmount}";
        }
    }

    public void ColectAllPoints(int amount)
    {
        if (amount > 0)
        {
            colectPoints.text = $"Colect Points \n{amount}";
        }
    }

    public void ColectPointsPerSecond(int amount)
    {
        if (amount > 0)
        {
            colectPerSecond.text = $"Colect Per Second \n{amount}";
        }
    }

    public void ClickPerSecondUpdateUI(int amount)
    {
        if (amount > 0)
        {
            clickPerSecond.text = $"Per Second \n{amount}";
        }
    }

    public void CounterClickUpdateUI(int amount)
    {
        if (amount > 0)
        {
            counterClick.text = $"Clicks \n{amount}";
        }
    }

    public void AmountPurchasesUpdateUI(int amount)
    {
        if (amount > 0)
        {
            amountPurchase.text = $"Purchase \n{amount}";
        }
    }

    public void CounterTimeInGame()
    {
        counterTimeStart += Time.deltaTime;
        int totalSeconds = Mathf.RoundToInt(counterTimeStart);

        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        if (hours > 0)
        {
            timerGame.text = $"Time \n{hours:D1}h {minutes:D1}m {seconds:D1}s";
        }
        else if (minutes > 0)
        {
            timerGame.text = $"Time \n{minutes:D1}m {seconds:D1}s";
        }
        else
        {
            timerGame.text = $"Time \n{seconds:D1}s";
        }
    }

    string FormatNumberWithDots(int number)
    {
        CultureInfo cultureInfo = new CultureInfo("pl-PL");
        cultureInfo.NumberFormat.NumberGroupSeparator = ".";
        return number.ToString("N0", cultureInfo);
    }
}
