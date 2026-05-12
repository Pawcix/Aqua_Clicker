using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CritComboChain : MonoBehaviour
{
    public static CritComboChain Instance;

    [Header("References:")]
    [SerializeField] System_Data data;
    [SerializeField] Slider comboSlider;
    [SerializeField] TextMeshProUGUI comboMultiplierText;
    [SerializeField] TextMeshProUGUI lastRewardText;
    [SerializeField] GameObject comboUIContainer;

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
        if (lastRewardText != null) lastRewardText.text = "";
        UpdateUI();
    }

    void Update()
    {
        if (!isComboActive) return;

        currentTimer -= Time.deltaTime;
        comboSlider.value = currentTimer / baseDuration;

        if (currentTimer <= 0) EndCombo();
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

        comboUIContainer.transform.localScale = Vector3.one * 1.25f;
        UpdateUI();
    }

    public void OnNormalClickRegistered()
    {
        if (!isComboActive) return;

        currentTimer -= penaltyOnNormalClick;
        comboUIContainer.transform.localScale = Vector3.one * 0.85f;
    }

    void StartCombo()
    {
        isComboActive = true;
        currentMultiplier = 1.0;
        accumulatedPointsInCritCombo = 0;
        currentTimer = baseDuration;
        if (comboUIContainer != null) comboUIContainer.SetActive(true);
        if (lastRewardText != null) lastRewardText.text = "";
    }

    void EndCombo()
    {
        isComboActive = false;

        double bonusReward = accumulatedPointsInCritCombo * (currentMultiplier - 1.0);

        if (bonusReward > 0)
        {
            data.pointsCounterFloat += bonusReward;

            if (lastRewardText != null)
            {
                StopAllCoroutines();
                string formatted = NumberFormatter.FormatWithDots(bonusReward);
                lastRewardText.text = $"CRIT BONUS: <color=yellow>+{formatted}</color>";
                StartCoroutine(FadeOutReward(3.5f));
            }
        }

        if (comboUIContainer != null) comboUIContainer.SetActive(false);
    }

    IEnumerator FadeOutReward(float duration)
    {
        lastRewardText.alpha = 1f;
        yield return new WaitForSeconds(duration);

        float fade = 1f;
        while (fade > 0)
        {
            fade -= Time.deltaTime;
            lastRewardText.alpha = fade;
            yield return null;
        }
        lastRewardText.text = "";
    }

    public double GetCurrentMultiplier() => isComboActive ? currentMultiplier : 1.0;

    void UpdateUI()
    {
        if (comboMultiplierText != null)
            comboMultiplierText.text = "CRIT X " + currentMultiplier.ToString("F2");

        if (currentMultiplier > data.highestCritMultiplier)
        {
            data.highestCritMultiplier = currentMultiplier;

            Clicker_Prefabs cp = Object.FindFirstObjectByType<Clicker_Prefabs>();
            if (cp != null) cp.UpdateAllPrefabs(data.pointsCounterFloat, data.pointsPerSecond);
        }

        comboUIContainer.transform.localScale = Vector3.one * 1.25f;
    }
}