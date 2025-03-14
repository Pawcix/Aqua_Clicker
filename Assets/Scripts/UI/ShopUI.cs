using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Header("Another UI Buttons")]
    [SerializeField] Button vanishButtonSettings;
    [SerializeField] Button vanishButtonStats;
    
    [SerializeField] RectTransform shop;
    [SerializeField] RectTransform content;

    [Header("Workers")]
    [SerializeField] ShopWorkerUI shopWorkerUI;

    public void AddWorker(Worker worker)
    {
        var newWorker = Instantiate(shopWorkerUI, content);
        newWorker.UpdateUI(worker);
    }

    public void OpenShop()
    {
        VanishCloseButtons();
        AudioManager.Instance.PlaySFX("Open");
    }

    public void CloseShop()
    {
        AppearCloseButtons();
        AudioManager.Instance.PlaySFX("Open");
    }

    void VanishCloseButtons()
    {
        vanishButtonSettings.gameObject.SetActive(false);
        vanishButtonStats.gameObject.SetActive(false);
    }

    void AppearCloseButtons()
    {
        vanishButtonSettings.gameObject.SetActive(true);
        vanishButtonStats.gameObject.SetActive(true);
    }
}
