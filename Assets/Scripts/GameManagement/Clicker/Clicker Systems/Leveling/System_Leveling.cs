using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Settings:")]
    [SerializeField] float xpDifficultyMultiplier = 1.25f;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        UpdateLevelUI();
    }

    public void RegisterPointGain(double amount)
    {
        data.currentXP += amount;

        if (data.currentXP >= data.xpToNextLevel)
        {
            LevelUp();
        }

        UpdateLevelUI();
    }

    void LevelUp()
    {
        if (data.xpToNextLevel < 1) data.xpToNextLevel = 100;

        int loopSafety = 0;

        while (data.currentXP >= data.xpToNextLevel && loopSafety < 100)
        {
            data.currentXP -= data.xpToNextLevel;
            data.currentLevel++;
            data.skillPoints++;

            data.xpToNextLevel *= xpDifficultyMultiplier;

            loopSafety++;
        }
    }

    public void UpdateLevelUI()
    {
        if (xpSlider != null)
        {
            xpSlider.maxValue = (float)data.xpToNextLevel;
            xpSlider.value = (float)data.currentXP;
        }

        if (levelText != null)
            levelText.text = "LVL: " + data.currentLevel;

        if (skillPointsText != null)
            skillPointsText.text = "Skill Points: " + data.skillPoints;

        if (percentText != null)
        {
            double percentage = (data.currentXP / data.xpToNextLevel) * 100;
            percentText.text = percentage.ToString("F1") + "%";
        }
    }
}
