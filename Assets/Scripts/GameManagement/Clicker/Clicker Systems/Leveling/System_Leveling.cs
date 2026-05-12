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
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI percentText;
    [SerializeField] TextMeshProUGUI skillPointsText;

    [Header("Level Up Notification:")]
    [SerializeField] TextMeshProUGUI levelUpText;
    [SerializeField] float displayDuration = 3f;

    [Header("Settings:")]
    [SerializeField] float xpDifficultyMultiplier = 1.25f;
    [SerializeField] float lerpSpeed = 5f;

    float currentVisualXP;

    void Awake()
    {
        if (Instance == null) Instance = this;
        if (levelUpText != null) levelUpText.gameObject.SetActive(false);
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

    void LevelUp()
    {
        if (data.xpToNextLevel < 1) data.xpToNextLevel = 100;

        int loopSafety = 0;
        bool leveledUp = false;

        while (data.currentXP >= data.xpToNextLevel && loopSafety < 100)
        {
            data.currentXP -= data.xpToNextLevel;
            data.currentLevel++;
            data.skillPoints++;
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

        if (skillPointsText != null)
            skillPointsText.text = "SKILL POINTS: " + data.skillPoints;
    }

    void ShowLevelUpNotification()
    {
        if (levelUpText != null)
        {
            StopAllCoroutines();
            StartCoroutine(LevelUpRoutine());
        }
    }

    IEnumerator LevelUpRoutine()
    {
        levelUpText.text = "LEVEL UP!";
        levelUpText.gameObject.SetActive(true);
        levelUpText.transform.localScale = Vector3.one * 0.5f;
        float elapsed = 0;
        while (elapsed < 0.2f)
        {
            elapsed += Time.deltaTime;
            levelUpText.transform.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.one, elapsed / 0.2f);
            yield return null;
        }
        yield return new WaitForSeconds(displayDuration);
        levelUpText.gameObject.SetActive(false);
    }
}