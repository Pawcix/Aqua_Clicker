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

    private bool isSFXOn = true;
    private float lastVolume = 1.0f;

    void Start()
    {
        if (sfxSlider != null) lastVolume = sfxSlider.value;
        UpdateTargetButtonSprites();
    }

    public void ToggleSFX()
    {
        isSFXOn = !isSFXOn;

        if (sfxSlider != null)
        {
            if (isSFXOn)
            {
                sfxSlider.value = lastVolume;
            }
            else
            {
                lastVolume = sfxSlider.value;
                sfxSlider.value = 0f;
            }
        }

        SFXVolume();
        UpdateTargetButtonSprites();

        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Modal - Open and Close");
        if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnSliderChanged(float value)
    {
        if (value > 0) lastVolume = value;
        AudioManager.Instance.SFXVolume(value);
    }

    public void SFXVolume()
    {
        if (sfxSlider != null)
        {
            AudioManager.Instance.SFXVolume(sfxSlider.value);
        }
    }

    void UpdateTargetButtonSprites()
    {
        if (sfxToggleButton == null) return;
        sfxToggleButton.transition = Selectable.Transition.SpriteSwap;
        sfxToggleButton.image.sprite = isSFXOn ? sfxOnSprite : sfxOffSprite;

        SpriteState state = new SpriteState();
        state.highlightedSprite = isSFXOn ? sfxOnSpriteHighlighted : sfxOffSpriteHighlighted;
        state.pressedSprite = isSFXOn ? sfxOnSprite : sfxOffSprite;
        state.selectedSprite = isSFXOn ? sfxOnSprite : sfxOffSprite;
        sfxToggleButton.spriteState = state;
    }
}