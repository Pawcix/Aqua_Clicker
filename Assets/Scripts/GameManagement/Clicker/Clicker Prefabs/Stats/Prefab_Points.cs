using TMPro;
using UnityEngine;

public class Prefab_Points : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI totalPointsText;

    public void UpdateTotalPointsPrefab(double amount)
    {
        if (totalPointsText == null) return;

        string formattedValue = NumberFormatter.FormatWithDots(amount);

        totalPointsText.text = $"Points \n{formattedValue}";
    }
}


