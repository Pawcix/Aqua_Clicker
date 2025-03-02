using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] Button vanishButtonSettings;
    [SerializeField] Button vanishButtonShop;
    [SerializeField] RectTransform stats;

    public void OpenStats()
    {
        stats.gameObject.SetActive(true);
        VanishCloseButtons();
        AudioManager.Instance.PlaySFX("Open");
    }

    public void CloseStats()
    {
        stats.gameObject.SetActive(false);
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
