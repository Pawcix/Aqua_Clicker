using TMPro;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class System_RiskReward : MonoBehaviour
{
    [Header("Data Source")]
    [SerializeField] System_Data data;

    [Header("UI References (Parents/Boxes)")]
    [SerializeField] GameObject riskWindowPanel;
    [SerializeField] GameObject openButton;
    [SerializeField] GameObject timerBox;
    [SerializeField] GameObject resultBox;

    [Header("UI Text References")]
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI resultText;

    [Header("Animation Reference")]
    [SerializeField] UI_CardFlipAnimation cardFlipAnimation;
    [SerializeField] RiskReward riskRewardVisuals;

    [Header("Buttons Game References")]
    [SerializeField] Button buttonA;
    [SerializeField] Button buttonB;

    [Header("Settings")]
    [SerializeField] int minutesWait = 60;

    bool canPlay = false;
    string pendingResultString = "";
    string pendingSFXName = "";
    float calculatedBonusDuration = 0f;

    void Start()
    {
        CheckTimer();
        UpdateUIState();
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
        return canPlay;
    }

    public void OpenRiskWindow()
    {
        if (riskWindowPanel != null) riskWindowPanel.SetActive(true);

        CheckTimer();
        UpdateUIState();
    }

    void UpdateUIState()
    {
        if (buttonA != null) buttonA.interactable = canPlay;
        if (buttonB != null) buttonB.interactable = canPlay;

        if (canPlay)
        {
            if (resultBox != null) resultBox.SetActive(false);
            if (timerBox != null) timerBox.SetActive(false);
        }
        else
        {
            if (timerBox != null) timerBox.SetActive(true);

            if (!string.IsNullOrEmpty(data.lastRiskResultText))
            {
                if (resultBox != null) resultBox.SetActive(true);
                if (resultText != null) resultText.text = data.lastRiskResultText;
            }
            else
            {
                if (resultBox != null) resultBox.SetActive(false);
            }
        }
    }

    public void PlayerMakeChoice()
    {
        if (!canPlay) return;

        if (riskRewardVisuals != null)
        {
            riskRewardVisuals.LockBoosterForAnimation();
        }

        bool isWin = UnityEngine.Random.Range(0, 2) == 1;

        if (isWin)
        {
            data.riskMultiplier = 2.0f;
            pendingResultString = "<color=#00FF00>SUCCESS!\nPPS x2.0</color>";
            pendingSFXName = "Risk - Success";
        }
        else
        {
            data.riskMultiplier = 0.5f;
            pendingResultString = "<color=#FF0000>FAILED!\nPPS x0.5</color>";
            pendingSFXName = "Risk - Failed";
        }

        if (buttonA != null) buttonA.interactable = false;
        if (buttonB != null) buttonB.interactable = false;

        calculatedBonusDuration = minutesWait * 60f;

        if (resultBox != null) resultBox.SetActive(true);

        if (cardFlipAnimation != null)
        {
            StartCoroutine(SafeStartAnimationRoutine());
        }
        else
        {
            ApplyFinalRewardAndStartCooldown();
        }
    }

    IEnumerator SafeStartAnimationRoutine()
    {
        yield return new WaitForEndOfFrame();
        if (cardFlipAnimation != null)
        {
            cardFlipAnimation.StartFlipAnimation(pendingResultString);
        }
    }

    public void ApplyFinalRewardAndStartCooldown()
    {
        if (AudioManager.Instance != null && !string.IsNullOrEmpty(pendingSFXName))
        {
            AudioManager.Instance.PlaySFX(pendingSFXName);
        }

        data.riskBonusTimer = calculatedBonusDuration;
        data.lastRiskResultText = pendingResultString;
        PlayerPrefs.SetString("LastRiskResultText", data.lastRiskResultText);

        canPlay = false;
        DateTime nextTime = DateTime.Now.AddMinutes(minutesWait);
        PlayerPrefs.SetString("NextRiskRewardPlay", nextTime.ToString());
        PlayerPrefs.Save();

        if (riskRewardVisuals != null)
        {
            riskRewardVisuals.StartBoosterDisplay();
        }

        if (System_NotificationRiskReward.Instance != null)
        {
            System_NotificationRiskReward.Instance.SetAlert(false);
        }

        if (resultBox != null) resultBox.SetActive(true);
        if (resultText != null) resultText.text = data.lastRiskResultText;
        if (timerBox != null) timerBox.SetActive(true);

        UpdateUIState();
    }

    void CheckTimer()
    {
        if (PlayerPrefs.HasKey("LastRiskResultText"))
        {
            data.lastRiskResultText = PlayerPrefs.GetString("LastRiskResultText");
        }

        if (!PlayerPrefs.HasKey("NextRiskRewardPlay")) canPlay = true;
        else canPlay = DateTime.Now >= DateTime.Parse(PlayerPrefs.GetString("NextRiskRewardPlay"));

        if (openButton != null) openButton.SetActive(true);
    }

    void UpdateTimerUI()
    {
        if (!PlayerPrefs.HasKey("NextRiskRewardPlay")) return;

        DateTime nextPlayTime = DateTime.Parse(PlayerPrefs.GetString("NextRiskRewardPlay"));
        TimeSpan timeRemaining = nextPlayTime - DateTime.Now;

        if (timeRemaining.TotalSeconds <= 0)
        {
            canPlay = true;
            data.lastRiskResultText = "";
            PlayerPrefs.DeleteKey("LastRiskResultText");
            PlayerPrefs.Save();

            UpdateUIState();
        }
        else
        {
            canPlay = false;
            if (timerText != null)
            {
                if (timeRemaining.TotalHours >= 1)
                {
                    timerText.text = string.Format("Next Play: {0:D2}:{1:D2}:{2:D2}",
                        (int)timeRemaining.TotalHours, timeRemaining.Minutes, timeRemaining.Seconds);
                }
                else
                {
                    timerText.text = string.Format("Next Play: {0:D2}:{1:D2}",
                        timeRemaining.Minutes, timeRemaining.Seconds);
                }
            }
        }
    }

    void ResetRiskMultiplier()
    {
        data.riskMultiplier = 1.0f;
        data.riskBonusTimer = 0;

        data.lastRiskResultText = "<color=#FFFFFF>EFFECT ENDED</color>";
        PlayerPrefs.SetString("LastRiskResultText", data.lastRiskResultText);
        PlayerPrefs.Save();

        if (riskWindowPanel != null && riskWindowPanel.activeInHierarchy)
        {
            UpdateUIState();
        }
    }
}