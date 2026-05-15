using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class System_RiskReward : MonoBehaviour
{
    [Header("Data Source")]
    [SerializeField] System_Data data;

    [Header("UI References")]
    [SerializeField] GameObject riskWindowPanel;
    [SerializeField] GameObject openButton;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] TextMeshProUGUI fortuneText;

    [Header("Buttons Troll References:")]
    [SerializeField] Button buttonA;
    [SerializeField] Button buttonB;
    [SerializeField] TextMeshProUGUI buttonAText;
    [SerializeField] TextMeshProUGUI buttonBText;
    [SerializeField] Image buttonAImage;
    [SerializeField] Image buttonBImage;

    [Header("Settings")]
    [SerializeField] int minutesWait = 60;
    [SerializeField] float bonusDuration = 900f;

    bool canPlay = false;

    List<string> buttonLabels = new List<string>()
    {
        "CLICK ME", "NOT THIS ONE", "THIS ONE", "MAYBE?", "YES!", "NO?", "50/50", "SAFE", "RISK", "LOSE?", "WIN?"
    };

    List<Color> trollColors = new List<Color>()
    {
        Color.green, Color.red, Color.yellow, Color.cyan, new Color(1f, 0.5f, 0f), Color.magenta
    };

    List<string> funnyFortunes = new List<string>()
    {
        "The stars alignment predicts a 100% chance of... something happening.",
        "Your financial horoscope today: High risk of losing points, but the text is green, so it's fine.",
        "A wise programmer once said: 'Clicking this button won't crash the engine. Probably.'",
        "Your luck level today: Equivalent to a wet cardboard box. Good luck!",
        "My calculations show that if you don't click, you won't win. Trust me, I'm an AI.",
        "Warning: This button has been blessed by the Spoon God. Proceed with chaotic energy.",
        "An old legend says that the left button is always luckier. Or was it the right one? I forgot.",
        "Don't worry, the developer simulated this 10,000 times and you won every time. (Lie)",
        "The universe whispers: 'Just do it, bro. What's the worst that could happen?'"
    };

    void Start()
    {
        if (resultText != null) resultText.text = "";
        if (riskWindowPanel != null) riskWindowPanel.SetActive(false);
        CheckTimer();
    }

    void Update()
    {
        if (!canPlay) UpdateTimerUI();

        if (data.riskBonusTimer > 0)
        {
            data.riskBonusTimer -= Time.deltaTime;
            if (data.riskBonusTimer <= 0) ResetRiskMultiplier();
        }
    }

    public bool CanPlayerPlay()
    {
        return true;
    }

    public void OpenRiskWindow()
    {
        if (riskWindowPanel != null) riskWindowPanel.SetActive(true);

        bool componentsActive = canPlay;
        if (buttonA != null) buttonA.gameObject.SetActive(componentsActive);
        if (buttonB != null) buttonB.gameObject.SetActive(componentsActive);

        if (canPlay && fortuneText != null && funnyFortunes.Count > 0)
        {
            if (resultText != null) resultText.text = "";
            int randomIndex = UnityEngine.Random.Range(0, funnyFortunes.Count);
            fortuneText.text = $"<color=#FFA500>FORTUNE COOKIE SAY:</color>\n<i>\"{funnyFortunes[randomIndex]}\"</i>";
            ApplyButtonTrolling();
        }
        else if (!canPlay)
        {
            if (fortuneText != null) fortuneText.text = "<i>\"Come back later, the cosmic forces are resting.\"</i>";
        }
    }

    void ApplyButtonTrolling()
    {
        if (buttonA == null || buttonB == null) return;

        buttonA.interactable = true;
        buttonB.interactable = true;

        if (buttonAText != null && buttonBText != null && buttonLabels.Count > 1)
        {
            int idxA = UnityEngine.Random.Range(0, buttonLabels.Count);
            int idxB = UnityEngine.Random.Range(0, buttonLabels.Count);
            while (idxB == idxA) idxB = UnityEngine.Random.Range(0, buttonLabels.Count);

            buttonAText.text = buttonLabels[idxA];
            buttonBText.text = buttonLabels[idxB];
        }

        if (buttonAImage != null && buttonBImage != null && trollColors.Count > 1)
        {
            int colA = UnityEngine.Random.Range(0, trollColors.Count);
            int colB = UnityEngine.Random.Range(0, trollColors.Count);
            while (colB == colA) colB = UnityEngine.Random.Range(0, trollColors.Count);

            buttonAImage.color = trollColors[colA];
            buttonBImage.color = trollColors[colB];
        }

        RectTransform rectA = buttonA.GetComponent<RectTransform>();
        RectTransform rectB = buttonB.GetComponent<RectTransform>();

        if (rectA != null && rectB != null)
        {
            if (UnityEngine.Random.Range(0, 2) == 1)
            {
                Vector3 posA = rectA.anchoredPosition;
                Vector3 posB = rectB.anchoredPosition;

                rectA.anchoredPosition = new Vector3(posB.x, posA.y, posA.z);
                rectB.anchoredPosition = new Vector3(posA.x, posB.y, posB.z);
            }
        }
    }

    public void PlayerMakeChoice()
    {
        if (!canPlay) return;

        buttonA.interactable = false;
        buttonB.interactable = false;

        bool isWin = UnityEngine.Random.Range(0, 2) == 1;

        if (isWin)
        {
            data.riskMultiplier = 2.0f;
            if (resultText != null) resultText.text = "<color=#00FF00>SUCCESS! PPS x2.0</color>";
            if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Risk_Win");
        }
        else
        {
            data.riskMultiplier = 0.5f;
            if (resultText != null) resultText.text = "<color=#FF0000>FAILED! PPS x0.5</color>";
            if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Risk_Lose");
        }

        data.riskBonusTimer = bonusDuration;

        canPlay = false;
        DateTime nextTime = DateTime.Now.AddMinutes(minutesWait);
        PlayerPrefs.SetString("NextRiskRewardPlay", nextTime.ToString());
        PlayerPrefs.Save();

        if (buttonA != null) buttonA.gameObject.SetActive(false);
        if (buttonB != null) buttonB.gameObject.SetActive(false);

        if (System_NotificationRiskReward.Instance != null)
        {
            System_NotificationRiskReward.Instance.SetAlert(false);
        }

        CheckTimer();
    }

    void CheckTimer()
    {
        if (!PlayerPrefs.HasKey("NextRiskRewardPlay")) canPlay = true;
        else canPlay = DateTime.Now >= DateTime.Parse(PlayerPrefs.GetString("NextRiskRewardPlay"));

        if (openButton != null) openButton.SetActive(true);
        if (timerText != null) timerText.gameObject.SetActive(true);
    }

    void UpdateTimerUI()
    {
        if (!PlayerPrefs.HasKey("NextRiskRewardPlay")) return;

        DateTime nextPlayTime = DateTime.Parse(PlayerPrefs.GetString("NextRiskRewardPlay"));
        TimeSpan timeRemaining = nextPlayTime - DateTime.Now;

        if (timeRemaining.TotalSeconds <= 0)
        {
            canPlay = true;
            if (timerText != null) timerText.text = "RISK READY!";

            if (riskWindowPanel != null && riskWindowPanel.activeInHierarchy)
            {
                OpenRiskWindow();
            }
        }
        else
        {
            canPlay = false;
            if (timerText != null)
            {
                if (timeRemaining.TotalHours >= 1)
                {
                    timerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                        (int)timeRemaining.TotalHours, timeRemaining.Minutes, timeRemaining.Seconds);
                }
                else
                {
                    timerText.text = string.Format("{0:D2}:{1:D2}",
                        timeRemaining.Minutes, timeRemaining.Seconds);
                }
            }
        }
    }

    void ResetRiskMultiplier()
    {
        data.riskMultiplier = 1.0f;
        data.riskBonusTimer = 0;
        if (resultText != null && riskWindowPanel.activeInHierarchy)
            resultText.text = "<color=#FFFFFF>EFFECT ENDED</color>";
    }
}