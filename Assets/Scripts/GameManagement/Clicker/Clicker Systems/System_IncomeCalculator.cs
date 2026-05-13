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

    void Update()
    {
        UpdateIncomeFormula();
    }

    void UpdateIncomeFormula()
    {
        if (data == null || formulaText == null) return;

        double workersTotalPPS = data.basePPS + data.workersPPS;

        if (workersTotalPPS < 0.01)
        {
            formulaText.text = "<size=80%>PASSIVE INCOME (PPS)</size>\n<color=#FF4444>NO PASSIVE INCOME</color>";
            if (breakdownText != null)
                breakdownText.text = "<size=70%>Buy workers to start earning automatically!</size>";
            return;
        }

        double finalPPS = workersTotalPPS * data.currentDailyMultiplier * data.wheelMultiplier;
        if (data.isGoldRushActive) finalPPS *= 2.0;
        if (ComboChain.Instance != null) finalPPS *= ComboChain.Instance.GetCurrentMultiplier();

        StringBuilder formula = new StringBuilder("<size=80%>PASSIVE INCOME (PPS)</size>\n");

        string finalPPSStr = finalPPS < 10 ? finalPPS.ToString("F1") : NumberFormatter.FormatWithDots(finalPPS);
        string workersPPSStr = workersTotalPPS < 10 ? workersTotalPPS.ToString("F1") : NumberFormatter.FormatWithDots(workersTotalPPS);

        formula.Append($"<color=#00FF00>{finalPPSStr}</color> = ");
        formula.Append($"<color={colBase}>{workersPPSStr}</color>");

        StringBuilder breakdown = new StringBuilder();
        breakdown.Append($"<color={colBase}>WORKERS: {workersPPSStr}</color>\n");

        if (data.currentDailyMultiplier > 1.0f)
        {
            string val = data.currentDailyMultiplier.ToString("F1");
            formula.Append($" * <color={colDaily}>{val}</color>");
            breakdown.Append($"<color={colDaily}>DAILY BONUS: x{val}</color>\n");
        }

        if (data.wheelMultiplier > 1.0f)
        {
            string val = data.wheelMultiplier.ToString("F1");
            formula.Append($" * <color={colWheel}>{val}</color>");
            breakdown.Append($"<color={colWheel}>WHEEL BOOST: x{val}</color>\n");
        }

        if (data.isGoldRushActive)
        {
            formula.Append($" * <color={colGold}>2.0</color>");
            breakdown.Append($"<color={colGold}>GOLD RUSH: x2.0</color>\n");
        }

        if (ComboChain.Instance != null && ComboChain.Instance.GetCurrentMultiplier() > 1.0)
        {
            string val = ComboChain.Instance.GetCurrentMultiplier().ToString("F2");
            formula.Append($" * <color={colCombo}>{val}</color>");
            breakdown.Append($"<color={colCombo}>COMBO CHAIN: x{val}</color>\n");
        }

        formulaText.text = formula.ToString();
        breakdownText.text = breakdown.ToString();
    }
}