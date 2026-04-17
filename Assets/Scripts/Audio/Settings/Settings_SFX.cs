using UnityEngine;
using UnityEngine.UI;

public class Settings_SFX : MonoBehaviour
{
    public Slider sfxSlider;

    [Header("SFX Settings:")]
    public Button sfxToggleButton;
    public Sprite sfxOnSprite;
    public Sprite sfxOffSprite;

    bool isSFXOn = true;

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

    public void SFXVolume()
    {
        if (sfxSlider != null)
        {
            AudioManager.Instance.SFXVolume(sfxSlider.value);
        }
    }
}

