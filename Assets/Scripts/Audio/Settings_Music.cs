using UnityEngine;
using UnityEngine.UI;

public class Settings_Music : MonoBehaviour
{
    public Slider musicSlider;

    [Header("Music Settings:")]
    public Button musicToggleButton;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;

    bool isMusicOn = true;

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

    public void MusicVolume()
    {
        if (musicSlider != null)
        {
            AudioManager.Instance.MusicVolume(musicSlider.value);
        }
    }
}
