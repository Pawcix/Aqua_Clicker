using TMPro;
using UnityEngine;

public class CritComboChain : MonoBehaviour
{
    public static CritComboChain Instance;

    [Header("References:")]
    [SerializeField] System_Data data;
    [SerializeField] TextMeshProUGUI comboMultiplierText;
    [SerializeField] TextMeshProUGUI comboSubText;
    [SerializeField] GameObject comboUIContainer;

    [Header("Prefab Score Spawn Settings:")]
    [SerializeField] GameObject comboScorePrefab;
    [SerializeField] Transform spawnContainer;

    [Header("Visual & Scaling Settings:")]
    [SerializeField] float pulseReturnSpeed = 10f;
    [SerializeField] float scaleIncreasePerHit = 0.02f;
    [SerializeField] float maxPermanentScale = 1.6f;
    float currentBaseScale = 1.0f;

    [Header("Text Timer Gradient Settings:")]
    [SerializeField] Color activeTimeColor = new Color(0.91f, 0.31f, 0.25f, 1f);
    [SerializeField] Color drainedTimeColor = new Color(0.15f, 0.15f, 0.15f, 1f);
    [SerializeField][Range(0.01f, 0.5f)] float gradientSharpness = 0.1f;

    [Header("Settings:")]
    [SerializeField] double multiplierStep = 0.05;
    [SerializeField] float baseDuration = 3.0f;
    [SerializeField] float penaltyOnNormalClick = 0.6f;

    float currentTimer;
    double currentMultiplier = 1.0;
    double accumulatedPointsInCritCombo = 0;
    bool isComboActive = false;

    void Awake() { Instance = this; }

    void Start()
    {
        if (comboUIContainer != null) comboUIContainer.SetActive(false);

        if (comboMultiplierText != null)
        {
            comboMultiplierText.enableVertexGradient = true;
        }

        if (comboSubText != null) comboSubText.text = "CRIT COMBO";

        UpdateUI();
    }

    void Update()
    {
        if (comboUIContainer == null || !comboUIContainer.activeSelf) return;

        if (comboUIContainer.transform.localScale.x > currentBaseScale || comboUIContainer.transform.localScale.x < currentBaseScale)
        {
            comboUIContainer.transform.localScale = Vector3.Lerp(
                comboUIContainer.transform.localScale,
                Vector3.one * currentBaseScale,
                Time.deltaTime * pulseReturnSpeed
            );
        }

        if (!isComboActive) return;

        currentTimer -= Time.deltaTime;

        float timeRatio = Mathf.Clamp01(currentTimer / baseDuration);
        ApplyTextTimeEffect(timeRatio);

        if (currentTimer <= 0) EndCombo();
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

    public void OnCritRegistered(double pointsGained)
    {
        if (!isComboActive)
        {
            StartCombo();
        }

        currentMultiplier += multiplierStep;
        accumulatedPointsInCritCombo += pointsGained;
        currentTimer = Mathf.Min(baseDuration, currentTimer + 0.4f);

        if (currentBaseScale < maxPermanentScale)
        {
            currentBaseScale += scaleIncreasePerHit;
        }

        UpdateUI();

        if (comboUIContainer != null)
        {
            float punchFactor = 1.2f + (scaleIncreasePerHit * 3f);
            comboUIContainer.transform.localScale = Vector3.one * (currentBaseScale * punchFactor);
        }
    }

    public void OnNormalClickRegistered()
    {
        if (!isComboActive || comboUIContainer == null || !comboUIContainer.activeSelf) return;

        currentTimer -= penaltyOnNormalClick;

        if (currentTimer <= 0f)
        {
            EndCombo();
            return;
        }

        comboUIContainer.transform.localScale = Vector3.one * (currentBaseScale * 0.85f);
        float timeRatio = Mathf.Clamp01(currentTimer / baseDuration);
        ApplyTextTimeEffect(timeRatio);
    }

    void StartCombo()
    {
        isComboActive = true;
        currentMultiplier = 1.0;
        accumulatedPointsInCritCombo = 0;
        currentTimer = baseDuration;
        currentBaseScale = 1.0f;

        if (comboUIContainer != null)
        {
            comboUIContainer.SetActive(true);
            comboUIContainer.transform.localScale = Vector3.one;
        }

        ApplyTextTimeEffect(1f);
    }

    void EndCombo()
    {
        isComboActive = false;
        currentBaseScale = 1.0f;

        double bonusReward = accumulatedPointsInCritCombo * (currentMultiplier - 1.0);

        if (bonusReward > 0)
        {
            data.pointsCounterFloat += bonusReward;

            if (comboScorePrefab != null && spawnContainer != null)
            {
                GameObject scoreClone = Instantiate(comboScorePrefab, spawnContainer);
                var anim = scoreClone.GetComponent<FloatingComboRewardText>();

                if (anim != null)
                {
                    string formatted = NumberFormatter.FormatWithDots(bonusReward);
                    anim.Setup(formatted);
                }
            }

            if (PointsDisplay.Instance != null) PointsDisplay.Instance.PulseCritComboEnd();
        }

        if (comboUIContainer != null) comboUIContainer.SetActive(false);
    }

    public double GetCurrentMultiplier() => isComboActive ? currentMultiplier : 1.0;

    void UpdateUI()
    {
        if (comboMultiplierText != null)
            comboMultiplierText.text = "X" + currentMultiplier.ToString("F2");

        if (currentMultiplier > data.highestCritMultiplier)
        {
            data.highestCritMultiplier = currentMultiplier;

            Clicker_Prefabs cp = Object.FindFirstObjectByType<Clicker_Prefabs>();
            if (cp != null) cp.UpdateAllPrefabs(data.pointsCounterFloat, data.pointsPerSecond);
        }
    }
}