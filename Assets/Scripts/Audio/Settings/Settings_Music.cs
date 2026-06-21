using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Settings_Music : MonoBehaviour
{
    public Slider musicSlider;

    [Header("Music Base Sprites:")]
    public Button musicToggleButton;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;

    [Header("Music Highlighted Sprites:")]
    public Sprite musicOnSpriteHighlighted;
    public Sprite musicOffSpriteHighlighted;

    private bool isMusicOn = true;
    private float lastVolume = 1.0f;

    void Start()
    {
        if (musicSlider != null) lastVolume = musicSlider.value;
        UpdateTargetButtonSprites();
    }

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;

        if (musicSlider != null)
        {
            if (isMusicOn)
            {
                musicSlider.value = lastVolume;
            }
            else
            {
                lastVolume = musicSlider.value;
                musicSlider.value = 0f;
            }
        }

        MusicVolume();
        UpdateTargetButtonSprites();

        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Modal - Open and Close");
        if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnSliderChanged(float value)
    {
        if (value > 0) lastVolume = value;
        AudioManager.Instance.MusicVolume(value);
    }

    public void MusicVolume()
    {
        if (musicSlider != null)
        {
            AudioManager.Instance.MusicVolume(musicSlider.value);
        }
    }

    void UpdateTargetButtonSprites()
    {
        if (musicToggleButton == null) return;
        musicToggleButton.transition = Selectable.Transition.SpriteSwap;
        musicToggleButton.image.sprite = isMusicOn ? musicOnSprite : musicOffSprite;

        SpriteState state = new SpriteState();
        state.highlightedSprite = isMusicOn ? musicOnSpriteHighlighted : musicOffSpriteHighlighted;
        state.pressedSprite = isMusicOn ? musicOnSprite : musicOffSprite;
        state.selectedSprite = isMusicOn ? musicOnSprite : musicOffSprite;
        musicToggleButton.spriteState = state;
    }
}