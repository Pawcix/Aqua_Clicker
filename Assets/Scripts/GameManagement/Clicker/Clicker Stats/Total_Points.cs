using TMPro;
using UnityEngine;

public class Total_Points : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI totalPointsText;

    public void UpdateDisplay(int amount)
    {
        if (totalPointsText == null) return;

        string formattedValue = NumberFormatter.FormatWithDots(amount);
        totalPointsText.text = $"Waters \n{formattedValue}";
    }
}
