using TMPro;
using UnityEngine;

public class Prefab_PPS : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI totalPSSText;

    public void UpdatePPS(double amount)
    {
        if (totalPSSText == null) return;

        string formattedValue = NumberFormatter.FormatWithDots(amount);
        totalPSSText.text = $"Per Second \n{formattedValue} /s";
    }
}
