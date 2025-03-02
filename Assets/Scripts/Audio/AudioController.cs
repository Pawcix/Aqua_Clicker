using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public Slider musicSlider, sfxSlider;

    [Header("Music")]
    public Button musicToggleButton;
    public Sprite musicOnImage;
    public Sprite musicOffImage;

    [Header("SFX")]
    public Button sfxToogleButton;
    public Sprite sfxOnImage;
    public Sprite sfxOffImage;

    bool isMusicOn = true;
    bool isSFXOn = true;

    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusic();
        isMusicOn = !isMusicOn;

        if (isMusicOn)
        {
            musicToggleButton.image.sprite = musicOnImage;
        }
        else
        {
            musicToggleButton.image.sprite = musicOffImage;
        }
    }

    public void ToggleSFX()
    {
        AudioManager.Instance.ToggleSFX();
        isSFXOn = !isSFXOn;

        if (isSFXOn)
        {
            sfxToogleButton.image.sprite = sfxOnImage;
        }
        else
        {
            sfxToogleButton.image.sprite = sfxOffImage;
        }
    }

    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume(musicSlider.value);
    }

    public void SFXVolume()
    {
        AudioManager.Instance.MusicVolume(sfxSlider.value);
    }
}
