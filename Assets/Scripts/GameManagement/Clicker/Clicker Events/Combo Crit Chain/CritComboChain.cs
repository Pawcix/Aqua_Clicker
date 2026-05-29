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
    [SerializeField] float pulseAmount = 1.05f;
    [SerializeField] float pulseReturnSpeed = 10f;
    [SerializeField] float scaleIncreasePerHit = 0.005f;
    [SerializeField] float maxPermanentScale = 1.5f;
    float currentBaseScale = 1.0f;

    [Header("Text Timer Gradient Settings (Klepsydra):")]
    [SerializeField] Color activeTimeColor = new Color(0.91f, 0.31f, 0.25f, 1f);
    [SerializeField] Color drainedTimeColor = new Color(0.15f, 0.15f, 0.15f, 1f);
    [SerializeField][Range(0.01f, 0.5f)] float gradientSharpness = 0.1f;

    [Header("New Crit Combo Settings:")]
    [SerializeField] float comboDuration = 15.0f;

    int totalCritClicks = 0;
    float currentTimer;
    double currentMultiplier = 1.0;
    double accumulatedPointsInCritCombo = 0;
    bool isComboActive = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (comboUIContainer != null) comboUIContainer.SetActive(false);
        if (comboMultiplierText != null) comboMultiplierText.enableVertexGradient = true;
        if (comboSubText != null) comboSubText.enableVertexGradient = true;
        if (comboSubText != null) comboSubText.text = "CRIT COMBO";

        UpdateUI();
    }

    void Update()
    {
        if (comboUIContainer != null && comboUIContainer.activeSelf)
        {
            if (comboUIContainer.transform.localScale.x > currentBaseScale || comboUIContainer.transform.localScale.x < currentBaseScale)
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

        float timeRatio = Mathf.Clamp01(currentTimer / comboDuration);
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
            comboSubText.colorGradient = gradient;
        }
    }

    public void OnCritRegistered(double pointsGained)
    {
        if (!isComboActive)
        {
            StartCombo();
        }

        totalCritClicks++;
        accumulatedPointsInCritCombo += pointsGained;

        currentMultiplier += 0.01;

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
    }

    public void OnNormalClickRegistered()
    {
        // Puste - czas ubywa liniowo, nic go nie szturcha przy zwykłym kliknięciu
    }

    void StartCombo()
    {
        isComboActive = true;
        currentMultiplier = 1.0;
        accumulatedPointsInCritCombo = 0;
        totalCritClicks = 0;
        currentTimer = comboDuration;
        currentBaseScale = 1.0f;

        if (comboUIContainer != null)
        {
            comboUIContainer.SetActive(true);
            comboUIContainer.transform.localScale = Vector3.one;
        }

        ApplyTextTimeEffect(1f);
        UpdateUI();
    }

    void EndCombo()
    {
        isComboActive = false;

        double baseReward = accumulatedPointsInCritCombo * currentMultiplier;
        double finalBonusReward = 0;

        if (baseReward > 0 && totalCritClicks > 0)
        {
            finalBonusReward = baseReward * totalCritClicks;
        }

        if (finalBonusReward > 0)
        {
            data.pointsCounterFloat += finalBonusReward;

            if (comboScorePrefab != null && spawnContainer != null)
            {
                GameObject scoreClone = Instantiate(comboScorePrefab, spawnContainer);
                var anim = scoreClone.GetComponent<FloatingComboRewardText>();
                if (anim != null)
                {
                    string formattedReward = NumberFormatter.FormatWithDots(finalBonusReward);
                    anim.Setup(formattedReward);
                }
            }

            if (PointsDisplay.Instance != null) PointsDisplay.Instance.PulseCritComboEnd();
        }

        totalCritClicks = 0;
        currentBaseScale = 1.0f;

        if (comboUIContainer != null) comboUIContainer.SetActive(false);
    }

    public double GetCurrentMultiplier() => isComboActive ? currentMultiplier : 1.0;

    void UpdateUI()
    {
        if (comboMultiplierText != null)
        {
            comboMultiplierText.SetText("X{0:0.00}", (float)currentMultiplier);
            comboMultiplierText.ForceMeshUpdate();
        }
    }
}