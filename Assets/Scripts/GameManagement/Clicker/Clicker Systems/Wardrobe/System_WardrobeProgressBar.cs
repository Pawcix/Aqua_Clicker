using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class System_WardrobeProgressBar : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] System_Wardrobe wardrobeManager;

    [Header("UI Elements:")]
    [SerializeField] TextMeshProUGUI progressText;

    public void UpdateProgressBar()
    {
        int totalSkins = wardrobeManager.GetAllSkins().Count;
        int unlockedSkins = wardrobeManager.GetUnlockedSkinsCount();

        if (totalSkins == 0) return;

        float progress01 = (float)unlockedSkins / totalSkins;
        float percentage = progress01 * 100f;

        if (progressText != null)
        {
            progressText.text = $"Skins Collected:\n{unlockedSkins} / {totalSkins} ({percentage:F0}%)";
        }
    }
}
