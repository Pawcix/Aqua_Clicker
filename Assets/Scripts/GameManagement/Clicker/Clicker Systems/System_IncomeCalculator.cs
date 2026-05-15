using TMPro;
using UnityEngine;
using System.Text;

public class System_IncomeCalculator : MonoBehaviour
{
    [SerializeField] System_Data data;
    [SerializeField] TextMeshProUGUI formulaText;
    [SerializeField] TextMeshProUGUI breakdownText;

    string colBase = "#FFFFFF";
    string colDaily = "#00FF00";
    string colWheel = "#00FFFF";
    string colGold = "#FFD700";
    string colCombo = "#FF4500";
    string colRisk = "#FF7F50";

    void Update()
    {
        UpdateIncomeFormula();
    }

    void UpdateIncomeFormula()
    {
        if (data == null || formulaText == null) return;

        double workersTotalPPS = data.basePPS + data.workersPPS;
        double comboMult = ComboChain.Instance != null ? ComboChain.Instance.GetCurrentMultiplier() : 1.0;
        double goldRushMult = data.isGoldRushActive ? 2.0 : 1.0;

        double finalPPS;
        if (workersTotalPPS < 0.01)
        {
            finalPPS = 1.0 * data.currentDailyMultiplier * data.wheelMultiplier * goldRushMult * comboMult * data.riskMultiplier;
        }
        else
        {
            finalPPS = workersTotalPPS * data.currentDailyMultiplier * data.wheelMultiplier * goldRushMult * comboMult * data.riskMultiplier;
        }

        bool hasAnyModifier = data.currentDailyMultiplier > 1.0f ||
                              data.wheelMultiplier > 1.0f ||
                              data.isGoldRushActive ||
                              comboMult > 1.0 ||
                              data.riskMultiplier != 1.0f; ;

        if (workersTotalPPS < 0.01 && !hasAnyModifier)
        {
            formulaText.text = "<size=80%>PASSIVE INCOME (PPS)</size>\n<color=#FF4444>NO PASSIVE INCOME</color>";
            if (breakdownText != null)
                breakdownText.text = "<size=70%>Buy workers to start earning automatically!</size>";
            return;
        }

        StringBuilder formula = new StringBuilder("<size=80%>PASSIVE INCOME (PPS)</size>\n");
        StringBuilder breakdown = new StringBuilder();

        string finalPPSStr = finalPPS < 10 ? finalPPS.ToString("F1") : NumberFormatter.FormatWithDots(finalPPS);
        formula.Append($"<color=#00FF00>{finalPPSStr}</color> = ");

        bool firstElementAdded = false;

        if (workersTotalPPS >= 0.01)
        {
            string workersPPSStr = workersTotalPPS < 10 ? workersTotalPPS.ToString("F1") : NumberFormatter.FormatWithDots(workersTotalPPS);
            formula.Append($"<color={colBase}>{workersPPSStr}</color>");
            breakdown.Append($"<color={colBase}>WORKERS: {workersPPSStr}</color>\n");
            firstElementAdded = true;
        }

        void AddMultiplier(double val, string color, string label)
        {
            if (val != 1.0)
            {
                string valStr = val.ToString("F1");
                if (firstElementAdded) formula.Append(" * ");

                formula.Append($"<color={color}>{valStr}</color>");
                breakdown.Append($"<color={color}>{label}: x{valStr}</color>\n");
                firstElementAdded = true;
            }
        }

        AddMultiplier(data.currentDailyMultiplier, colDaily, "DAILY BONUS");
        AddMultiplier(data.wheelMultiplier, colWheel, "WHEEL BOOST");
        AddMultiplier(goldRushMult, colGold, "GOLD RUSH");
        AddMultiplier(comboMult, colCombo, "COMBO CHAIN");
        AddMultiplier(data.riskMultiplier, colRisk, "RISK REWARD");

        formulaText.text = formula.ToString();
        breakdownText.text = breakdown.ToString();
    }
}