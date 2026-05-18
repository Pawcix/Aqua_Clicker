using TMPro;
using UnityEngine;

public class Prefab_Rebirth : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI totalRebirthsText;

    public void UpdateTotalRebirthsPrefab(int total)
    {
        if (totalRebirthsText == null) return;

        string formattedValue = NumberFormatter.FormatWithDots(total);
        totalRebirthsText.text = $"Rebirths\n{formattedValue}";
    }
}
