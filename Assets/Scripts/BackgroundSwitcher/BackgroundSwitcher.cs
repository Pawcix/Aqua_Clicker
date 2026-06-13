using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BackgroundSwitcher : MonoBehaviour
{
    [Header("UI Elements")]
    public Image backgroundImage;
    public Image previewImage;

    [Header("Data")]
    public System_Data dataSystem;
    public List<Sprite> backgroundList;

    private int currentIndex = 0;

    void Start()
    {
        currentIndex = dataSystem.currentBackground;

        if (currentIndex < 0 || currentIndex >= backgroundList.Count)
            currentIndex = 0;

        UpdateDisplays();
    }

    public void NextBackground()
    {
        currentIndex++;
        if (currentIndex >= backgroundList.Count) currentIndex = 0;

        SaveAndDisplay();
    }

    public void PreviousBackground()
    {
        currentIndex--;
        if (currentIndex < 0) currentIndex = backgroundList.Count - 1;

        SaveAndDisplay();
    }

    void SaveAndDisplay()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Modal - Open and Close");

        dataSystem.currentBackground = currentIndex;
        UpdateDisplays();
    }

    public void UpdateDisplays()
    {
        backgroundImage.sprite = backgroundList[currentIndex];
        previewImage.sprite = backgroundList[currentIndex];
    }
}