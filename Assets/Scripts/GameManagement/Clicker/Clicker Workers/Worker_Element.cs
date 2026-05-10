using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Worker_Element : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI powerText;
    [SerializeField] TextMeshProUGUI priceBuyText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI totalBonusText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] Button buyButton;
    [SerializeField] Image workerImage;

    System_Data data;
    Worker workerTemplate;
    int workerID;
    int cachedAmount;
    double cachedTotalPrice;

    void Update()
    {
        if (buyButton != null && data != null && workerTemplate != null)
        {
            CalculateCurrentBulkStats();

            bool canAfford = data.pointsCounterFloat >= cachedTotalPrice && cachedAmount > 0;

            if (buyButton.interactable != canAfford)
            {
                buyButton.interactable = canAfford;
            }

            if (Worker_PurchaseSettings.CurrentMode == Worker_PurchaseSettings.PurchaseMode.Max)
            {
                UpdatePriceText();
            }
        }
    }

    public void Setup(Worker worker, int id, System_Data dataRef)
    {
        workerTemplate = worker;
        workerID = id;
        data = dataRef;

        UpdateUI();

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyWorker);
    }

    void CalculateCurrentBulkStats()
    {
        int currentLevel = data.workerLevels[workerID];

        switch (Worker_PurchaseSettings.CurrentMode)
        {
            case Worker_PurchaseSettings.PurchaseMode.x1:
                cachedAmount = 1;
                break;
            case Worker_PurchaseSettings.PurchaseMode.x5:
                cachedAmount = 5;
                break;
            case Worker_PurchaseSettings.PurchaseMode.Max:
                cachedAmount = workerTemplate.GetMaxAffordable(currentLevel, data.pointsCounterFloat);
                if (cachedAmount <= 0) cachedAmount = 1;
                break;
        }

        cachedTotalPrice = workerTemplate.GetTotalCost(currentLevel, cachedAmount);
    }

    void BuyWorker()
    {
        CalculateCurrentBulkStats();

        if (data.pointsCounterFloat >= cachedTotalPrice && cachedAmount > 0)
        {
            data.pointsCounterFloat -= cachedTotalPrice;

            float totalPowerGained = workerTemplate.basePower * cachedAmount;

            Clicker_System.OnItemBought.Invoke(cachedTotalPrice, totalPowerGained);

            data.workerLevels[workerID] += cachedAmount;

            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        if (data == null || workerTemplate == null) return;

        int currentLevel = data.workerLevels[workerID];
        float totalPPS = currentLevel * workerTemplate.basePower;

        nameText.text = workerTemplate.workerName;
        powerText.text = "+" + workerTemplate.basePower.ToString("F1") + "/s";
        workerImage.sprite = workerTemplate.icon;

        if (levelText != null) levelText.text = "Lvl: " + currentLevel;
        if (totalBonusText != null) totalBonusText.text = "PPS: " + totalPPS.ToString("F1");
        if (descriptionText != null) descriptionText.text = workerTemplate.description;

        UpdatePriceText();
    }

    void UpdatePriceText()
    {
        CalculateCurrentBulkStats();

        if (priceBuyText == null) return;

        string finalButtonText = "";

        if (Worker_PurchaseSettings.CurrentMode == Worker_PurchaseSettings.PurchaseMode.Max)
        {
            finalButtonText = $"MAX ({cachedAmount})\n{NumberFormatter.FormatWithDots(cachedTotalPrice)}";
        }
        else
        {
            finalButtonText = NumberFormatter.FormatWithDots(cachedTotalPrice);
        }

        priceBuyText.text = finalButtonText;
    }
}