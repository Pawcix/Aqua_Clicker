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

    bool isMusicOn = true;

    void Start()
    {
        UpdateTargetButtonSprites();
    }

    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusic();
        isMusicOn = !isMusicOn;

        UpdateTargetButtonSprites();

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    void UpdateTargetButtonSprites()
    {
        if (musicToggleButton == null) return;

        musicToggleButton.transition = Selectable.Transition.SpriteSwap;

        if (musicOnSprite != null && musicOffSprite != null)
        {
            musicToggleButton.image.sprite = isMusicOn ? musicOnSprite : musicOffSprite;
        }

        SpriteState state = new SpriteState();

        if (isMusicOn)
        {
            state.highlightedSprite = musicOnSpriteHighlighted;
            state.pressedSprite = musicOnSprite;
            state.selectedSprite = musicOnSprite;
        }
        else
        {
            state.highlightedSprite = musicOffSpriteHighlighted;
            state.pressedSprite = musicOffSprite;
            state.selectedSprite = musicOffSprite;
        }

        musicToggleButton.spriteState = state;
    }

    public void MusicVolume()
    {
        if (musicSlider != null)
        {
            AudioManager.Instance.MusicVolume(musicSlider.value);
        }
    }
}