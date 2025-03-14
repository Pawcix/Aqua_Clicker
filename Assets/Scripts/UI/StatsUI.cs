using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [Header("Another UI Buttons")]
    [SerializeField] Button vanishButtonSettings;
    [SerializeField] Button vanishButtonShop;

    public void OpenStats()
    {
        VanishCloseButtons();
        AudioManager.Instance.PlaySFX("Open");
    }

    public void CloseStats()
    {
        AppearCloseButtons();
        AudioManager.Instance.PlaySFX("Open");
    }

    void VanishCloseButtons()
    {
        vanishButtonSettings.gameObject.SetActive(false);
        vanishButtonShop.gameObject.SetActive(false);
    }

    void AppearCloseButtons()
    {
        vanishButtonSettings.gameObject.SetActive(true);
        vanishButtonShop.gameObject.SetActive(true);
    }
}
