using TMPro;
using UnityEngine;

public class Prefab_ComboChain : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI maxComboText;

    public void UpdateMaxComboPrefab(double multiplier)
    {
        if (maxComboText == null) return;

        maxComboText.text = $"Best Combo \nx{multiplier.ToString("F2")}";
    }
}
