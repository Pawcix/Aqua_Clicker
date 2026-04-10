using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject settingsPanel;
    public Slider musicSlider, sfxSlider;

    [Header("Music Settings:")]
    public Button musicToggleButton;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;

    [Header("SFX Settings:")]
    public Button sfxToggleButton;
    public Sprite sfxOnSprite;
    public Sprite sfxOffSprite;

    bool isMusicOn = true;
    bool isSFXOn = true;

    void Awake()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettings();
        }
    }

    public void ToggleSettings()
    {
        if (settingsPanel != null)
        {
            bool newState = !settingsPanel.activeSelf;
            settingsPanel.SetActive(newState);
        }
    }

    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusic();
        isMusicOn = !isMusicOn;

        if (musicToggleButton != null && musicOnSprite != null && musicOffSprite != null)
        {
            musicToggleButton.image.sprite = isMusicOn ? musicOnSprite : musicOffSprite;
            // Debug.Log($"[Settings] Music is now: {(isMusicOn ? "ON" : "OFF")}");
        }
    }

    public void ToggleSFX()
    {
        AudioManager.Instance.ToggleSFX();
        isSFXOn = !isSFXOn;

        if (sfxToggleButton != null && sfxOnSprite != null && sfxOffSprite != null)
        {
            sfxToggleButton.image.sprite = isSFXOn ? sfxOnSprite : sfxOffSprite;
            // Debug.Log($"[Settings] SFX is now: {(isSFXOn ? "ON" : "OFF")}");
        }
    }

    public void MusicVolume()
    {
        if (musicSlider != null)
        {
            AudioManager.Instance.MusicVolume(musicSlider.value);
        }
    }

    public void SFXVolume()
    {
        if (sfxSlider != null)
        {
            AudioManager.Instance.SFXVolume(sfxSlider.value);
        }
    }
}