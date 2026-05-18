using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class System_Rebirth : MonoBehaviour
{
    [Header("Data Source:")]
    [SerializeField] System_Data data;

    [Header("Dependencies:")]
    [SerializeField] System_Leveling levelingSystem;
    [SerializeField] Clicker_Prefabs clickerPrefabs;

    [Header("UI Security Elements:")]
    [SerializeField] Button rebirthButton;
    [SerializeField] TextMeshProUGUI statusText;
    [SerializeField] TextMeshProUGUI cooldownTimerText;

    [Header("Cooldown Settings (Offline Time):")]
    [SerializeField] int baseCooldownMinutes = 10;
    [SerializeField] int minutesIncreasePerRebirth = 5;

    [Header("First Rebirth Requirements:")]
    [SerializeField] int requiredFirstLevel = 10;

    bool isCooldownActive = false;
    const string RebirthCooldownKey = "NextRebirthReadyTime";

    void Start()
    {
        CheckCooldown();
        UpdateUIElements();
    }

    void Update()
    {
        if (isCooldownActive)
        {
            UpdateTimerUI();
        }
        else
        {
            UpdateAvailableRebirthUI();
        }
    }

    void CheckCooldown()
    {
        if (!PlayerPrefs.HasKey(RebirthCooldownKey))
        {
            isCooldownActive = false;
            return;
        }

        string savedTimeStr = PlayerPrefs.GetString(RebirthCooldownKey);
        if (string.IsNullOrEmpty(savedTimeStr) || !DateTime.TryParse(savedTimeStr, out DateTime readyTime))
        {
            isCooldownActive = false;
            return;
        }

        isCooldownActive = DateTime.Now < readyTime;
    }

    void UpdateTimerUI()
    {
        if (readyTimeMissing()) return;

        string savedTimeStr = PlayerPrefs.GetString(RebirthCooldownKey);
        if (!DateTime.TryParse(savedTimeStr, out DateTime readyTime))
        {
            isCooldownActive = false;
            UpdateUIElements();
            return;
        }

        TimeSpan timeRemaining = readyTime - DateTime.Now;

        if (timeRemaining.TotalSeconds <= 0)
        {
            isCooldownActive = false;
            UpdateUIElements();
        }
        else
        {
            if (rebirthButton != null) rebirthButton.interactable = false;
            if (statusText != null) statusText.text = "<color=#FF4444>LOCKED</color>";

            string timeStr;
            if (timeRemaining.TotalHours >= 1)
            {
                timeStr = string.Format("{0:D2}:{1:D2}:{2:D2}",
                    (int)timeRemaining.TotalHours, timeRemaining.Minutes, timeRemaining.Seconds);
            }
            else
            {
                timeStr = string.Format("{0:D2}:{1:D2}",
                    timeRemaining.Minutes, timeRemaining.Seconds);
            }

            if (cooldownTimerText != null)
            {
                cooldownTimerText.text = $"NEXT REBIRTH IN:\n<color=#FF4444>{timeStr}</color>";
            }
        }
    }

    void UpdateAvailableRebirthUI()
    {
        if (readyTimeMissing()) return;

        if (data.rebirthCount == 0 && data.currentLevel < requiredFirstLevel)
        {
            if (rebirthButton != null) rebirthButton.interactable = false;
            if (statusText != null) statusText.text = "<color=#FF4444>LOCKED</color>";

            if (cooldownTimerText != null)
            {
                cooldownTimerText.text = $"FIRST REBIRTH REQ:\n<color=#FF4444>LEVEL {requiredFirstLevel} (YOU: {data.currentLevel})</color>";
            }
            return;
        }

        if (rebirthButton != null) rebirthButton.interactable = true;
        if (statusText != null) statusText.text = "<color=#00FF00>REBIRTH</color>";

        float potentialBonus = CalculatePotentialBonus();
        if (cooldownTimerText != null)
        {
            cooldownTimerText.text = $"CURRENT REWARD:\n<color=#FFD700>+{potentialBonus:F2}x MULTIPLIER</color>";
        }
    }

    void UpdateUIElements()
    {
        if (isCooldownActive) UpdateTimerUI();
        else UpdateAvailableRebirthUI();
    }

    bool readyTimeMissing()
    {
        return rebirthButton == null || statusText == null || cooldownTimerText == null;
    }

    float CalculatePotentialBonus()
    {
        return (data.currentLevel * data.currentLevel) * 0.001f;
    }

    public void ExecuteRebirth()
    {
        if (data == null || isCooldownActive) return;

        if (data.rebirthCount == 0 && data.currentLevel < requiredFirstLevel)
        {
            Debug.LogWarning($"Nie możesz jeszcze wykonać Rebirth! Wymagany poziom: {requiredFirstLevel}");
            return;
        }

        float bonusFromLevels = CalculatePotentialBonus();
        data.rebirthMultiplier += bonusFromLevels;
        data.rebirthCount++;

        int currentCooldownMinutes = baseCooldownMinutes + (data.rebirthCount * minutesIncreasePerRebirth);
        DateTime nextReadyTime = DateTime.Now.AddMinutes(currentCooldownMinutes);

        data.pointsCounterFloat = 0;
        data.currentLevel = 1;
        data.currentXP = 0;
        data.xpToNextLevel = 100;
        data.rebirthPoints = 0;
        data.basePPS = 0;
        data.workersPPS = 0;

        PlayerPrefs.SetString(RebirthCooldownKey, nextReadyTime.ToString());
        PlayerPrefs.SetInt("RebirthCount", data.rebirthCount);
        PlayerPrefs.SetFloat("RebirthMultiplier", data.rebirthMultiplier);
        PlayerPrefs.SetFloat("Points", 0f);
        PlayerPrefs.SetInt("CurrentLevel", 1);
        PlayerPrefs.SetFloat("CurrentXP", 0f);
        PlayerPrefs.SetFloat("XpToNextLevel", 100f);
        PlayerPrefs.SetInt("rebirthPoints", 0);
        PlayerPrefs.Save();

        isCooldownActive = true;

        if (levelingSystem != null) levelingSystem.UpdateLevelUI(true);
        if (clickerPrefabs != null) clickerPrefabs.UpdateAllPrefabs(0, 0);

        UpdateUIElements();

        System_RebirthJuice juiceEffect = GetComponent<System_RebirthJuice>();
        if (System_RebirthJuice.Instance != null)
        {
            System_RebirthJuice.Instance.TriggerRebirthEffects(data.rebirthMultiplier);
        }

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("Rebirth_Sound");
    }
}