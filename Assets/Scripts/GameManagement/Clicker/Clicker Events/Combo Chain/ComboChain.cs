using TMPro;
using UnityEngine;
using System.Collections;

public class ComboChain : MonoBehaviour
{
    public static ComboChain Instance;

    [Header("References:")]
    [SerializeField] System_Data data;
    [SerializeField] TextMeshProUGUI comboMultiplierText;
    [SerializeField] TextMeshProUGUI comboSubText;
    [SerializeField] GameObject comboUIContainer;

    [Header("Prefab Score Spawn Settings:")]
    [SerializeField] private GameObject comboScorePrefab;
    [SerializeField] private Transform spawnContainer;

    [Header("Visual Settings:")]
    [SerializeField] float pulseAmount = 1.05f;
    [SerializeField] float pulseReturnSpeed = 10f;
    [SerializeField] float scaleIncreasePerHit = 0.01f;
    [SerializeField] float maxPermanentScale = 1.5f;

    [Header("Text Timer Gradient Settings:")]
    [SerializeField] private Color activeTimeColor = new Color(1f, 0.07f, 0.57f, 1f);
    [SerializeField] private Color drainedTimeColor = new Color(0.15f, 0.15f, 0.15f, 1f);
    [SerializeField][Range(0.01f, 0.5f)] private float gradientSharpness = 0.1f;

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
        if (comboMultiplierText != null) comboMultiplierText.enableVertexGradient = true;
        if (comboSubText != null) comboSubText.text = "COMBO";

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

        float timeRatio = Mathf.Clamp01(currentTimer / maxTime);
        ApplyTextTimeEffect(timeRatio);

        if (currentTimer <= 0)
        {
            EndCombo();
        }
    }

    void ApplyTextTimeEffect(float timeRatio)
    {
        if (comboMultiplierText == null) return;

        VertexGradient gradient = new VertexGradient();
        float topT = Mathf.Clamp01((timeRatio - gradientSharpness) / (1f - gradientSharpness));
        gradient.topLeft = Color.Lerp(drainedTimeColor, activeTimeColor, topT);
        gradient.topRight = Color.Lerp(drainedTimeColor, activeTimeColor, topT);

        float bottomT = Mathf.Clamp01(timeRatio / (1f - gradientSharpness));
        gradient.bottomLeft = Color.Lerp(drainedTimeColor, activeTimeColor, bottomT);
        gradient.bottomRight = Color.Lerp(drainedTimeColor, activeTimeColor, bottomT);
        comboMultiplierText.colorGradient = gradient;

        if (comboSubText != null)
        {
            Color subColor = comboSubText.color;
            comboSubText.color = new Color(subColor.r, subColor.g, subColor.b, timeRatio);
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
            float punchFactor = pulseAmount + (scaleIncreasePerHit * 5f);
            comboUIContainer.transform.localScale = Vector3.one * (currentBaseScale * punchFactor);
        }

        if (Mastery.Instance != null)
        {
            Mastery.Instance.AddMasteryXP(Mastery.MasteryType.Combo, 2f);
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

            if (comboScorePrefab != null && spawnContainer != null)
            {
                GameObject scoreClone = Instantiate(comboScorePrefab, spawnContainer);

                var anim = scoreClone.GetComponent<FloatingComboRewardText>();
                if (anim != null)
                {
                    string formattedReward = NumberFormatter.FormatWithDots(bonusReward);
                    anim.Setup(formattedReward);
                }
            }

            if (PointsDisplay.Instance != null) PointsDisplay.Instance.PulseComboEnd();
        }

        if (comboUIContainer != null) comboUIContainer.SetActive(false);
    }

    float CalculateMaxTime()
    {
        return Mathf.Max(minDuration, baseDuration - (float)((currentMultiplier - 1.0) * 10.0 * difficultyScale));
    }

    void UpdateUI()
    {
        if (comboMultiplierText != null)
            comboMultiplierText.text = "x" + currentMultiplier.ToString("F2");
    }

    public double GetCurrentMultiplier()
    {
        return isComboActive ? currentMultiplier : 1.0;
    }
}