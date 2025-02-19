using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopWorkerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI powerText;
    [SerializeField] TextMeshProUGUI priceBuyText;
    [SerializeField] TextMeshProUGUI upgradeText;
    [SerializeField] TextMeshProUGUI upgradePowerText;
    [SerializeField] Button buyButton;
    [SerializeField] Button upgradeButton;

    Worker currentWorker;

    public void UpdateUI(Worker worker)
    {
        currentWorker = worker.Clone();

        nameText.text = currentWorker.name;
        powerText.text = "+" + currentWorker.power.ToString();
        upgradePowerText.text = "+" + currentWorker.powerUpgrade.ToString();
        priceBuyText.text = currentWorker.pricePower.ToString();
        upgradeText.text = currentWorker.priceUpgrade.ToString();

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(delegate { BuyButtonOnClick(currentWorker.pricePower, currentWorker.power); });

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(delegate { UpgradeButtonOnClick(currentWorker.priceUpgrade); });
    }

    void BuyButtonOnClick(int price, int power)
    {
        ClickerManager.OnItemBought?.Invoke(price, power);
    }

    void UpgradeButtonOnClick(int priceUpgrade)
    {
        ClickerManager.OnUpgradeItem?.Invoke(priceUpgrade, currentWorker);

        UpdateUI(currentWorker);
    }
}