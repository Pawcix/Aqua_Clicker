using TMPro;
using UnityEngine;

public class Prefab_PPS : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI totalPSSText;

    public void UpdatePPS(double amount)
    {
        if (totalPSSText == null) return;

        string formattedValue;

        if (amount < 10)
        {

            formattedValue = amount.ToString("F1");
        }

        else if (amount < 100)
        {
            formattedValue = amount.ToString("F0");
        }
        else
        {
            formattedValue = NumberFormatter.FormatWithDots(amount);
        }

        totalPSSText.text = $"PER SECOND\n{formattedValue} /S";
    }
}
