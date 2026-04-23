using TMPro;
using UnityEngine;

public class Total_PPS : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ppsText;

    public void UpdatePPS(int amount)
    {
        if (ppsText == null) return;

        string formattedValue = NumberFormatter.FormatWithDots(amount);
        ppsText.text = $"Per Second \n{formattedValue} /s";
    }
}
