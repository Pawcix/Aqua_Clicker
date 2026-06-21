using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class BackgroundSwitcher : MonoBehaviour
{
    [Header("UI Elements")]
    public Image backgroundImage;
    public Image previewImage;
    public TextMeshProUGUI backgroundNameText;

    [Header("Data")]
    public System_Data dataSystem;
    public List<Sprite> backgroundList;
    public List<string> backgroundNames;

    private int currentIndex = 0;

    void Start()
    {
        currentIndex = Mathf.Clamp(dataSystem.currentBackground, 0, backgroundList.Count - 1);
        UpdateDisplay();
    }

    public void NextBackground()
    {
        if (backgroundList.Count == 0) return;

        currentIndex = (currentIndex + 1) % backgroundList.Count;
        SaveAndDisplay();
    }

    public void PreviousBackground()
    {
        if (backgroundList.Count == 0) return;

        currentIndex = (currentIndex - 1 + backgroundList.Count) % backgroundList.Count;
        SaveAndDisplay();
    }

    void SaveAndDisplay()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Modal - Open and Close");

        dataSystem.currentBackground = currentIndex;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        if (backgroundImage != null && currentIndex < backgroundList.Count)
        {
            backgroundImage.sprite = backgroundList[currentIndex];
        }

        if (previewImage != null && currentIndex < backgroundList.Count)
        {
            previewImage.sprite = backgroundList[currentIndex];
        }

        if (backgroundNameText != null && currentIndex < backgroundNames.Count)
        {
            backgroundNameText.text = backgroundNames[currentIndex];
        }
    }
}