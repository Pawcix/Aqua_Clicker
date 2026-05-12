using TMPro;
using UnityEngine;

public class Prefab_ComboCrit : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI maxComboText;

    public void UpdateMaxComboPrefab(double multiplier)
    {
        if (maxComboText == null) return;

        maxComboText.text = $"Combo Crits Chain \nx{multiplier.ToString("F2")}";
    }
}
