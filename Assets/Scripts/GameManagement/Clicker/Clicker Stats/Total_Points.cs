using TMPro;
using UnityEngine;

public class Total_Points : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI totalPointsText;

    public void UpdateDisplay(double amount)
    {
        if (totalPointsText == null) return;

        string formattedValue = NumberFormatter.FormatWithDots(amount);
        totalPointsText.text = $"Waters \n{formattedValue}";
    }
}
