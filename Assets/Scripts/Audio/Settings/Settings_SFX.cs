using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Settings_SFX : MonoBehaviour
{
    public Slider sfxSlider;

    [Header("SFX Base Sprites:")]
    public Button sfxToggleButton;
    public Sprite sfxOnSprite;
    public Sprite sfxOffSprite;

    [Header("SFX Highlighted Sprites:")]
    public Sprite sfxOnSpriteHighlighted;
    public Sprite sfxOffSpriteHighlighted;

    bool isSFXOn = true;

    void Start()
    {
        UpdateTargetButtonSprites();
    }

    public void ToggleSFX()
    {
        AudioManager.Instance.ToggleSFX();
        isSFXOn = !isSFXOn;

        UpdateTargetButtonSprites();

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    void UpdateTargetButtonSprites()
    {
        if (sfxToggleButton == null) return;

        sfxToggleButton.transition = Selectable.Transition.SpriteSwap;

        if (sfxOnSprite != null && sfxOffSprite != null)
        {
            sfxToggleButton.image.sprite = isSFXOn ? sfxOnSprite : sfxOffSprite;
        }

        SpriteState state = new SpriteState();

        if (isSFXOn)
        {
            state.highlightedSprite = sfxOnSpriteHighlighted;
            state.pressedSprite = sfxOnSprite;
            state.selectedSprite = sfxOnSprite;
        }
        else
        {
            state.highlightedSprite = sfxOffSpriteHighlighted;
            state.pressedSprite = sfxOffSprite;
            state.selectedSprite = sfxOffSprite;
        }

        sfxToggleButton.spriteState = state;
    }

    public void SFXVolume()
    {
        if (sfxSlider != null)
        {
            AudioManager.Instance.SFXVolume(sfxSlider.value);
        }
    }
}