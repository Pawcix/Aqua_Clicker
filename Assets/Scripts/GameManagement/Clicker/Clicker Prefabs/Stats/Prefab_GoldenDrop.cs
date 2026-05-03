using TMPro;
using UnityEngine;

public class Prefab_GoldenDrop : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI totalGoldenDropsText;

    public void UpdateTotalGoldenDropsPrefab(int totalGoldenDrops)
    {
        if (totalGoldenDropsText == null) return;

        string formattedValue = NumberFormatter.FormatWithDots(totalGoldenDrops);
        totalGoldenDropsText.text = $"Golden Drops\n{formattedValue}";
    }
}
