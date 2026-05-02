using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ComboChain : MonoBehaviour
{
    public static ComboChain Instance;

    [Header("References:")]
    [SerializeField] System_Data data;
    [SerializeField] Slider comboSlider;
    [SerializeField] TextMeshProUGUI comboMultiplierText;
    [SerializeField] TextMeshProUGUI lastRewardText;
    [SerializeField] GameObject comboUIContainer;

    [Header("Visual Settings:")]
    [SerializeField] float pulseAmount = 1.05f;
    [SerializeField] float pulseReturnSpeed = 10f;
    [SerializeField] float scaleIncreasePerHit = 0.01f;
    [SerializeField] float maxPermanentScale = 1.5f;

    [Header("Combo Settings:")]
    [SerializeField] double multiplierStep = 0.01;
    [SerializeField] int clicksToActivate = 10;
    [SerializeField] float baseDuration = 2.0f;
    [SerializeField] float minDuration = 0.5f;
    [SerializeField] float difficultyScale = 0.02f;

    int clickCounter = 0;
    float currentTimer;
    double currentMultiplier = 1.0;
    double accumulatedPointsInCombo = 0;
    bool isComboActive = false;

    float currentBaseScale = 1.0f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (comboUIContainer != null) comboUIContainer.SetActive(false);
        if (lastRewardText != null) lastRewardText.text = "";
        UpdateUI();
    }

    void Update()
    {
        if (comboUIContainer != null && comboUIContainer.activeSelf)
        {
            if (comboUIContainer.transform.localScale.x > currentBaseScale)
            {
                comboUIContainer.transform.localScale = Vector3.Lerp(
                    comboUIContainer.transform.localScale,
                    Vector3.one * currentBaseScale,
                    Time.deltaTime * pulseReturnSpeed
                );
            }
        }

        if (!isComboActive) return;

        currentTimer -= Time.deltaTime;
        float maxTime = CalculateMaxTime();
        comboSlider.value = currentTimer / maxTime;

        if (currentTimer <= 0)
        {
            EndCombo();
        }
    }

    public void OnClickRegistered(double pointsGained)
    {
        if (!isComboActive)
        {
            clickCounter++;
            if (clickCounter >= clicksToActivate)
            {
                StartCombo();
            }
            return;
        }

        currentMultiplier += multiplierStep;
        accumulatedPointsInCombo += pointsGained;
        currentTimer = CalculateMaxTime();

        if (currentBaseScale < maxPermanentScale)
        {
            currentBaseScale += scaleIncreasePerHit;
        }

        UpdateUI();

        if (comboUIContainer != null)
        {
            comboUIContainer.transform.localScale = Vector3.one * (currentBaseScale * pulseAmount);
        }
    }

    void StartCombo()
    {
        isComboActive = true;
        currentMultiplier = 1.0;
        accumulatedPointsInCombo = 0;
        currentTimer = baseDuration;
        currentBaseScale = 1.0f;

        if (comboUIContainer != null)
        {
            comboUIContainer.SetActive(true);
            comboUIContainer.transform.localScale = Vector3.one;
        }
        if (lastRewardText != null) lastRewardText.text = "";
    }

    void EndCombo()
    {
        isComboActive = false;
        clickCounter = 0;
        currentBaseScale = 1.0f;

        double bonusReward = accumulatedPointsInCombo * (currentMultiplier - 1.0);

        if (bonusReward > 0)
        {
            data.pointsCounterFloat += bonusReward;

            if (lastRewardText != null)
            {
                StopAllCoroutines();
                string formattedReward = NumberFormatter.FormatWithDots(bonusReward);
                lastRewardText.text = "BONUS: +" + formattedReward;
                StartCoroutine(ShowRewardRoutine(3.0f));
            }

            if (PointsDisplay.Instance != null) PointsDisplay.Instance.Pulse();
        }

        if (comboUIContainer != null) comboUIContainer.SetActive(false);
    }

    IEnumerator ShowRewardRoutine(float duration)
    {
        lastRewardText.alpha = 1.0f;
        yield return new WaitForSeconds(duration);

        float fadeTimer = 0.5f;
        while (fadeTimer > 0)
        {
            fadeTimer -= Time.deltaTime;
            lastRewardText.alpha = fadeTimer / 0.5f;
            yield return null;
        }

        lastRewardText.text = "";
        lastRewardText.alpha = 1.0f;
    }

    float CalculateMaxTime()
    {
        return Mathf.Max(minDuration, baseDuration - (float)((currentMultiplier - 1.0) * 10.0 * difficultyScale));
    }

    void UpdateUI()
    {
        if (comboMultiplierText != null)
            comboMultiplierText.text = "COMBO X" + currentMultiplier.ToString("F2");

        if (currentMultiplier > data.highestComboMultiplier)
        {
            data.highestComboMultiplier = currentMultiplier;

            Clicker_Prefabs cp = Object.FindFirstObjectByType<Clicker_Prefabs>();
            if (cp != null) cp.UpdateAllPrefabs(data.pointsCounterFloat, data.pointsPerSecond);
        }
    }
}
