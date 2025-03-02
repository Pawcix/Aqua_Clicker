using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] Button vanishButtonStats;
    [SerializeField] Button vanishButtonShop;
    [SerializeField] RectTransform settings;

    public void OpenSettings()
    {
        settings.gameObject.SetActive(true);
        VanishCloseButtons();
        AudioManager.Instance.PlaySFX("Open");
    }

    public void CloseSettings()
    {
        settings.gameObject.SetActive(false);
        AppearCloseButtons();
        AudioManager.Instance.PlaySFX("Open");
    }

    void VanishCloseButtons()
    {
        vanishButtonStats.gameObject.SetActive(false);
        vanishButtonShop.gameObject.SetActive(false);
    }

    void AppearCloseButtons()
    {
        vanishButtonStats.gameObject.SetActive(true);
        vanishButtonShop.gameObject.SetActive(true);
    }
}
