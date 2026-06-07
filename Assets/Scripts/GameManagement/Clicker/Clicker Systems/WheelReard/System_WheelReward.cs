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
    [SerializeField] TextMeshProUGUI spinButtonText;
    [SerializeField] GameObject timerBox;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float idleRotationSpeed = 20f;

    [Header("Settings")]
    [SerializeField] List<WheelReward> rewards;
    [SerializeField] float spinDuration = 4f;
    [SerializeField] AnimationCurve spinCurve;
    [SerializeField] int minutesWait = 30;
    [SerializeField] float angleOffset = 0f;

    [SerializeField] float resultDisplayTime = 3f;

    bool isSpinning = false;
    bool isShowingResult = false;

    void Start()
    {
        UpdateWheelState();
    }

    void Update()
    {
        if (!isSpinning)
        {
            wheelCircle.Rotate(Vector3.forward, idleRotationSpeed * Time.deltaTime);

            if (!isShowingResult)
            {
                UpdateTimerLogic();
            }
        }
    }

    void UpdateTimerLogic()
    {
        if (!PlayerPrefs.HasKey("NextWheelSpin"))
        {
            spinButton.SetActive(true);
            spinButtonText.text = "SPIN";
            timerBox.SetActive(false);
            return;
        }

        DateTime nextSpinTime = DateTime.Parse(PlayerPrefs.GetString("NextWheelSpin"));
        TimeSpan timeRemaining = nextSpinTime - DateTime.Now;

        if (timeRemaining.TotalSeconds <= 0)
        {
            spinButton.SetActive(true);
            spinButtonText.text = "SPIN";
            timerBox.SetActive(false);
        }
        else
        {
            spinButton.SetActive(false);
            timerBox.SetActive(true);
            timerText.text = $"Next Spin: {timeRemaining.Minutes:D2}:{timeRemaining.Seconds:D2}";
        }
    }

    public void StartSpin()
    {
        if (isSpinning || isShowingResult) return;

        spinButtonText.text = "SPINNING...";

        int targetSliceIndex = UnityEngine.Random.Range(0, 8);
        float sliceSize = 45f;

        float targetAngle = (targetSliceIndex * sliceSize) + (sliceSize / 2f);
        float totalRotation = (360f * 5f) - targetAngle;

        StartCoroutine(SpinRoutine(totalRotation, targetSliceIndex));
    }

    IEnumerator SpinRoutine(float totalRotation, int targetSliceIndex)
    {
        isSpinning = true;
        spinButton.SetActive(false);
        timerBox.SetActive(false);

        float elapsed = 0;
        while (elapsed < spinDuration)
        {
            elapsed += Time.deltaTime;
            wheelCircle.eulerAngles = new Vector3(0, 0, -(totalRotation * spinCurve.Evaluate(elapsed / spinDuration)) + angleOffset);
            yield return null;
        }

        isSpinning = false;
        isShowingResult = true;

        DateTime nextTime = DateTime.Now.AddMinutes(minutesWait);
        PlayerPrefs.SetString("NextWheelSpin", nextTime.ToString());
        PlayerPrefs.Save();

        spinButton.SetActive(true);
        EvaluateFinalReward(targetSliceIndex);

        StartCoroutine(HideResultAfterDelay());
    }

    void EvaluateFinalReward(int finalSliceIndex)
    {
        WheelReward finalReward = (finalSliceIndex == 0) ? rewards[0] : rewards[1];
        spinButtonText.text = (finalSliceIndex == 0) ? "WIN\n " + finalReward.rewardName : "FAILED\n " + finalReward.rewardName;

        GiveReward(finalReward);
    }

    void GiveReward(WheelReward reward)
    {
        data.wheelMultiplier = (float)reward.value;
        data.wheelBonusTimer = minutesWait * 60f;
        data.currentWheelRewardIcon = reward.rewardIcon;
    }

    IEnumerator HideResultAfterDelay()
    {
        yield return new WaitForSeconds(resultDisplayTime);

        isShowingResult = false;
    }

    void UpdateWheelState()
    {
        isShowingResult = false;

        if (PlayerPrefs.HasKey("NextWheelSpin"))
        {
            DateTime nextSpin = DateTime.Parse(PlayerPrefs.GetString("NextWheelSpin"));
            if (DateTime.Now < nextSpin)
            {
                spinButton.SetActive(false);
                timerBox.SetActive(true);
                return;
            }
        }
        spinButton.SetActive(true);
        timerBox.SetActive(false);
    }
}