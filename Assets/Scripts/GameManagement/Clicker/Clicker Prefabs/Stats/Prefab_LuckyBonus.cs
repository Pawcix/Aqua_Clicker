using TMPro;
using UnityEngine;

public class Prefab_LuckyBonus : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI totalLuckyBonusesText;

    public void UpdateTotalLuckyBonusesPrefab(int total)
    {
        if (totalLuckyBonusesText == null) return;

        string formattedValue = NumberFormatter.FormatWithDots(total);
        totalLuckyBonusesText.text = $"Lucky Bonuses\n{formattedValue}";
    }
}
