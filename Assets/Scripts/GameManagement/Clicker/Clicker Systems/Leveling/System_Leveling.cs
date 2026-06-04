using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class System_Leveling : MonoBehaviour
{
    public static System_Leveling Instance;

    [Header("References:")]
    [SerializeField] System_Data data;

    [Header("UI Elements:")]
    [SerializeField] Slider xpSlider;
    [SerializeField] Image xpSliderFill;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI percentText;
    [SerializeField] TextMeshProUGUI rebirthPointsText;

    [Header("Level Up Notification:")]
    [SerializeField] TextMeshProUGUI levelUpText;
    [SerializeField] float displayDuration = 2.5f;

    [Header("Juicy Notification Settings:")]
    [SerializeField] float punchScale = 1.3f;
    [SerializeField] float idlePulseSpeed = 5f;
    [SerializeField] float idlePulseAmount = 0.04f;
    [SerializeField] float idleBobSpeed = 4f;
    [SerializeField] float idleBobAmount = 8f;
    [SerializeField] float fadeOutDuration = 0.4f;

    [Header("Settings:")]
    [SerializeField] float xpDifficultyMultiplier = 1.25f;
    [SerializeField] float lerpSpeed = 5f;
    [SerializeField] float colorFlashDuration = 0.6f;

    float currentVisualXP;
    Color originalFillColor;
    Color luckyBonusColor;
    Color goldenDropColor;
    Coroutine colorFlashCoroutine;
    Coroutine levelUpCoroutine;

    Vector3 originalNotificationPos;
    Color originalNotificationColor;

    void Awake()
    {
        if (Instance == null) Instance = this;

        if (levelUpText != null)
        {
            originalNotificationPos = levelUpText.transform.localPosition;
            originalNotificationColor = levelUpText.color;
            levelUpText.gameObject.SetActive(false);
        }

        if (xpSliderFill != null)
        {
            originalFillColor = xpSliderFill.color;
        }

        if (ColorUtility.TryParseHtmlString("#E84E40", out Color parsedLucky))
        {
            luckyBonusColor = parsedLucky;
        }

        if (ColorUtility.TryParseHtmlString("#FFCD00", out Color parsedGolden))
        {
            goldenDropColor = parsedGolden;
        }
    }

    void Start()
    {
        currentVisualXP = (float)data.currentXP;
        UpdateLevelUI(true);
    }

    void Update()
    {
        HandleSliderAnimation();
    }

    void HandleSliderAnimation()
    {
        if (xpSlider == null) return;

        currentVisualXP = Mathf.Lerp(currentVisualXP, (float)data.currentXP, Time.deltaTime * lerpSpeed);

        xpSlider.maxValue = (float)data.xpToNextLevel;
        xpSlider.value = currentVisualXP;

        if (percentText != null)
        {
            float percentage = (currentVisualXP / (float)data.xpToNextLevel) * 100f;
            percentText.text = percentage.ToString("F1") + "%";
        }
    }

    public void RegisterPointGain(double amount)
    {
        data.currentXP += amount;

        if (data.currentXP >= data.xpToNextLevel)
        {
            LevelUp();
        }

        UpdateLevelUI(false);
    }

    public void FlashLuckyBonusColor()
    {
        if (xpSliderFill == null) return;
        if (colorFlashCoroutine != null) StopCoroutine(colorFlashCoroutine);
        colorFlashCoroutine = StartCoroutine(FlashColorRoutine(luckyBonusColor));
    }

    public void FlashGoldenDropColor()
    {
        if (xpSliderFill == null) return;
        if (colorFlashCoroutine != null) StopCoroutine(colorFlashCoroutine);
        colorFlashCoroutine = StartCoroutine(FlashColorRoutine(goldenDropColor));
    }

    IEnumerator FlashColorRoutine(Color targetColor)
    {
        xpSliderFill.color = targetColor;
        float elapsed = 0f;

        while (elapsed < colorFlashDuration)
        {
            elapsed += Time.deltaTime;
            xpSliderFill.color = Color.Lerp(targetColor, originalFillColor, elapsed / colorFlashDuration);
            yield return null;
        }

        xpSliderFill.color = originalFillColor;
    }

    void LevelUp()
    {
        if (data.xpToNextLevel < 1) data.xpToNextLevel = 100;

        int loopSafety = 0;
        bool leveledUp = false;

        while (data.currentXP >= data.xpToNextLevel && loopSafety < 100)
        {
            data.currentXP -= data.xpToNextLevel;
            data.currentLevel++;
            data.rebirthPoints++;
            data.xpToNextLevel *= xpDifficultyMultiplier;

            leveledUp = true;
            loopSafety++;
        }

        if (leveledUp)
        {
            currentVisualXP = 0;
            ShowLevelUpNotification();
            if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("LevelUp");
        }
    }

    public void UpdateLevelUI(bool immediate)
    {
        if (immediate) currentVisualXP = (float)data.currentXP;

        if (levelText != null)
            levelText.text = "LVL: " + data.currentLevel;

        if (rebirthPointsText != null)
            rebirthPointsText.text = "REBIRTH POINTS: " + data.rebirthPoints;
    }

    void ShowLevelUpNotification()
    {
        if (levelUpText != null)
        {
            if (levelUpCoroutine != null) StopCoroutine(levelUpCoroutine);
            levelUpCoroutine = StartCoroutine(LevelUpRoutine());
        }
    }

    IEnumerator LevelUpRoutine()
    {
        levelUpText.color = originalNotificationColor;
        levelUpText.transform.localPosition = originalNotificationPos;
        levelUpText.transform.localScale = Vector3.zero;
        levelUpText.gameObject.SetActive(true);

        float elapsed = 0f;
        float introDuration = 0.2f;

        while (elapsed < introDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / introDuration;

            float currentScale = 0f;
            if (t < 0.6f)
            {
                currentScale = Mathf.Lerp(0f, punchScale, t / 0.6f);
            }
            else
            {
                currentScale = Mathf.Lerp(punchScale, 1.0f, (t - 0.6f) / 0.4f);
            }

            levelUpText.transform.localScale = Vector3.one * currentScale;
            yield return null;
        }
        levelUpText.transform.localScale = Vector3.one;

        float idleTimeRemaining = displayDuration - introDuration - fadeOutDuration;
        float idleElapsed = 0f;

        while (idleElapsed < idleTimeRemaining)
        {
            idleElapsed += Time.deltaTime;

            float yOffset = Mathf.Sin(Time.time * idleBobSpeed) * idleBobAmount;
            levelUpText.transform.localPosition = originalNotificationPos + new Vector3(0f, yOffset, 0f);

            float pulse = 1.0f + Mathf.Sin(Time.time * idlePulseSpeed) * idlePulseAmount;
            levelUpText.transform.localScale = Vector3.one * pulse;

            yield return null;
        }

        elapsed = 0f;
        Color startColor = levelUpText.color;
        Vector3 startPos = levelUpText.transform.localPosition;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeOutDuration;
            float newAlpha = Mathf.Lerp(1f, 0f, t);
            levelUpText.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);

            levelUpText.transform.localPosition += new Vector3(0f, Time.deltaTime * 60f, 0f);

            yield return null;
        }

        levelUpText.gameObject.SetActive(false);
    }
}