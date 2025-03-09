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
    [SerializeField] Button buyButton;
    [SerializeField] Image workerImage;

    Worker currentWorker;

    public void UpdateUI(Worker worker)
    {
        currentWorker = worker.Clone();

        nameText.text = currentWorker.name;
        powerText.text = "+" + currentWorker.power.ToString();
        priceBuyText.text = currentWorker.pricePower.ToString();

        workerImage.sprite = currentWorker.image;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(delegate { BuyButtonOnClick(currentWorker.pricePower, currentWorker.power); });
    }

    void BuyButtonOnClick(int price, int power)
    {
        ClickerManager.OnItemBought?.Invoke(price, power);
    }
}