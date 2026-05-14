using TMPro;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class System_WheelFortune : MonoBehaviour
{
    [Header("Data Source")]
    [SerializeField] System_Data data;

    [Header("UI References")]
    [SerializeField] RectTransform wheelCircle;
    [SerializeField] GameObject spinButton;
    [SerializeField] TextMeshProUGUI rewardNameText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float idleRotationSpeed = 20f;

    [Header("Settings")]
    [SerializeField] List<WheelReward> rewards;
    [SerializeField] float spinDuration = 4f;
    [SerializeField] AnimationCurve spinCurve;
    [SerializeField] int minutesWait = 30;

    [Header("Calibration")]
    [Range(0, 360)]
    [SerializeField] float angleOffset = 0f;

    bool isSpinning = false;
    bool canSpin = false;

    void Start()
    {
        if (rewardNameText != null) rewardNameText.text = "";
        CheckTimer();
    }

    void Update()
    {
        if (!isSpinning && !canSpin)
        {
            UpdateTimerUI();
        }

        if (!isSpinning)
        {
            wheelCircle.Rotate(Vector3.forward, idleRotationSpeed * Time.deltaTime);
        }
    }

    void CheckTimer()
    {
        if (!PlayerPrefs.HasKey("NextWheelSpin"))
        {
            canSpin = true;
        }
        else
        {
            DateTime nextSpinTime = DateTime.Parse(PlayerPrefs.GetString("NextWheelSpin"));
            canSpin = DateTime.Now >= nextSpinTime;
        }

        if (spinButton != null) spinButton.SetActive(canSpin);
    }

    void UpdateTimerUI()
    {
        if (!PlayerPrefs.HasKey("NextWheelSpin")) return;

        DateTime nextSpinTime = DateTime.Parse(PlayerPrefs.GetString("NextWheelSpin"));
        TimeSpan timeRemaining = nextSpinTime - DateTime.Now;

        if (timeRemaining.TotalSeconds <= 0)
        {
            canSpin = true;
            if (timerText != null) timerText.gameObject.SetActive(false);
            if (spinButton != null) spinButton.SetActive(true);
        }
        else
        {
            canSpin = false;
            if (spinButton != null) spinButton.SetActive(false);
            if (timerText != null)
            {
                timerText.gameObject.SetActive(true);
                timerText.text = string.Format("{0:D2}:{1:D2}", timeRemaining.Minutes, timeRemaining.Seconds);
            }
        }
    }

    public void StartSpin()
    {
        if (isSpinning || !canSpin || rewards.Count == 0) return;

        float randomFinalAngle = UnityEngine.Random.Range(0f, 360f);
        float totalRotation = (360f * 5f) + randomFinalAngle;

        StartCoroutine(SpinRoutine(totalRotation));
    }

    WheelReward ChooseWinner()
    {
        float totalChance = 0;
        foreach (var r in rewards) totalChance += r.chance;

        float randomVal = UnityEngine.Random.Range(0, totalChance);
        float currentSum = 0;

        foreach (var r in rewards)
        {
            currentSum += r.chance;
            if (randomVal <= currentSum)
            {
                return r;
            }
        }
        return rewards[0];
    }

    IEnumerator SpinRoutine(float totalRotation)
    {
        isSpinning = true;
        canSpin = false;
        if (spinButton != null) spinButton.SetActive(false);

        float elapsed = 0;
        float startAngle = wheelCircle.eulerAngles.z;

        while (elapsed < spinDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / spinDuration;
            float curveT = spinCurve.Evaluate(t);
            float currentRotation = startAngle + (totalRotation * curveT);
            wheelCircle.eulerAngles = new Vector3(0, 0, currentRotation);

            yield return null;
        }

        isSpinning = false;
        EvaluateFinalReward();

        DateTime nextTime = DateTime.Now.AddMinutes(minutesWait);
        PlayerPrefs.SetString("NextWheelSpin", nextTime.ToString());
        PlayerPrefs.Save();
    }

    void EvaluateFinalReward()
    {
        float rot = wheelCircle.eulerAngles.z % 360;
        if (rot < 0) rot += 360;

        int rewardIndex = 0;
        float snapAngle = 0;

        if (rot > 22.5f && rot <= 67.5f) { rewardIndex = 1; snapAngle = 45f; }
        else if (rot > 67.5f && rot <= 112.5f) { rewardIndex = 2; snapAngle = 90f; }
        else if (rot > 112.5f && rot <= 157.5f) { rewardIndex = 3; snapAngle = 135f; }
        else if (rot > 157.5f && rot <= 202.5f) { rewardIndex = 4; snapAngle = 180f; }
        else if (rot > 202.5f && rot <= 247.5f) { rewardIndex = 5; snapAngle = 225f; }
        else if (rot > 247.5f && rot <= 292.5f) { rewardIndex = 6; snapAngle = 270f; }
        else if (rot > 292.5f && rot <= 337.5f) { rewardIndex = 7; snapAngle = 315f; }
        else { rewardIndex = 0; snapAngle = 0f; }

        wheelCircle.eulerAngles = new Vector3(0, 0, snapAngle + 22.5f + angleOffset);

        if (rewardIndex < rewards.Count)
        {
            WheelReward winner = rewards[rewardIndex];
            if (rewardNameText != null) rewardNameText.text = "YOU WON: " + winner.rewardName;
            GiveReward(winner);
        }
    }

    void GiveReward(WheelReward reward)
    {
        if (reward.type == WheelReward.RewardType.Points)
        {
            float bonusPercent = (float)reward.value;

            data.wheelMultiplier = 1.0f + bonusPercent;
            data.wheelBonusTimer = 600f;
            data.currentWheelRewardIcon = reward.rewardIcon;
        }
    }
}