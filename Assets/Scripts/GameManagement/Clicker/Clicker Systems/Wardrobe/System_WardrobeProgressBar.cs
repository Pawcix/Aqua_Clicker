using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class System_WardrobeProgressBar : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] System_Wardrobe wardrobeManager;

    [Header("UI Elements:")]
    [SerializeField] TextMeshProUGUI progressText;

    void Start()
    {
        Invoke(nameof(UpdateProgressBar), 0.1f);
    }

    void OnEnable()
    {
        Invoke(nameof(UpdateProgressBar), 0.05f);
    }

    public void UpdateProgressBar()
    {
        if (wardrobeManager == null || wardrobeManager.data == null) return;

        List<ClickerSkin> allSkins = wardrobeManager.GetAllSkins();
        int totalSkins = allSkins.Count;

        if (totalSkins == 0) return;

        int unlockedSkins = 0;

        foreach (var skin in allSkins)
        {
            if (wardrobeManager.data.unlockedSkinIDs.Contains(skin.skinID))
            {
                unlockedSkins++;
            }
        }

        float progress01 = (float)unlockedSkins / totalSkins;
        float percentage = progress01 * 100f;

        if (progressText != null)
        {
            progressText.text = $"Skins Collected:\n{unlockedSkins} / {totalSkins} ({percentage:F0}%)";
        }
    }
}