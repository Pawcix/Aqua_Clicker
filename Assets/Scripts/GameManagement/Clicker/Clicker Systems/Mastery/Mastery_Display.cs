using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mastery_Display : MonoBehaviour
{
    [SerializeField] Mastery.MasteryType type;
    [SerializeField] System_Data data;
    [SerializeField] Slider xpSlider;
    [SerializeField] TextMeshProUGUI lvlText;
    [SerializeField] TextMeshProUGUI bonusText;
    [SerializeField] TextMeshProUGUI progressPercentText;

    [SerializeField] int maxLevel = 200;
    [SerializeField] Color maxLevelColor = Color.yellow;
    [SerializeField] Color normalColor = Color.white;

    void Update()
    {
        if (data == null) return;
        UpdateUI();
    }

    void UpdateUI()
    {
        int lvl = 0;
        float xp = 0;
        float needed = 100;
        string label = "";
        float bonusValue = 0;

        switch (type)
        {
            case Mastery.MasteryType.Click:
                lvl = data.clickMasteryLvl;
                xp = data.clickMasteryXP;
                needed = 100f * Mathf.Pow(1.10f, lvl);
                bonusValue = lvl * 0.5f;
                label = "\nCLICK POWER";
                break;

            case Mastery.MasteryType.Critical:
                lvl = data.critMasteryLvl;
                xp = data.critMasteryXP;
                needed = 200f * Mathf.Pow(1.15f, lvl);
                bonusValue = lvl * 1.0f;
                label = "\nCRIT MULTI";
                break;

            case Mastery.MasteryType.Combo:
                lvl = data.comboMasteryLvl;
                xp = data.comboMasteryXP;
                needed = 150f * Mathf.Pow(1.12f, lvl);
                bonusValue = lvl * 1.0f;
                label = "\nCOMBO POWER";
                break;

            case Mastery.MasteryType.AwayIncome:
                lvl = data.awayMasteryLvl;
                xp = data.awayMasteryXP;
                needed = 250f * Mathf.Pow(1.12f, lvl);
                bonusValue = lvl * 1.0f;
                label = "\nAWAY PROFITS";
                break;
        }

        bool isMax = lvl >= maxLevel;

        if (lvlText != null)
        {
            lvlText.text = isMax ? "MAX LEVEL" : "LVL: " + lvl;
            lvlText.color = isMax ? maxLevelColor : normalColor;
        }

        if (bonusText != null)
        {
            bonusText.text = $"+{bonusValue:F1}% {label}";
            bonusText.color = isMax ? maxLevelColor : normalColor;
        }

        if (xpSlider != null)
        {
            xpSlider.maxValue = needed;
            xpSlider.value = isMax ? needed : xp;
        }

        if (progressPercentText != null)
        {
            if (isMax)
            {
                progressPercentText.text = "MAXED";
                progressPercentText.color = maxLevelColor;
            }
            else
            {
                float percentage = (xp / needed) * 100f;
                progressPercentText.text = percentage.ToString("F1") + "%";
                progressPercentText.color = normalColor;
            }
        }
    }
}
