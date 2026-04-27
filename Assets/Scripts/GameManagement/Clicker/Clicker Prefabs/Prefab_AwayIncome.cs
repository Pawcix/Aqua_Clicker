using TMPro;
using UnityEngine;

public class Prefab_AwayIncome : MonoBehaviour
{
    [SerializeField] System_Data data;
    [SerializeField] TextMeshProUGUI totalAwayText;

    void Start()
    {
        UpdateTotalDisplay();
    }

    public void UpdateTotalDisplay()
    {
        if (totalAwayText == null || data == null) return;

        string formattedValue = NumberFormatter.FormatWithDots(data.totalAwayEarnings);
        totalAwayText.text = $"Total Away Profits: \n{formattedValue}";
    }

    void OnEnable()
    {
        UpdateTotalDisplay();
    }
}
