using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Worker_Element : MonoBehaviour
{
    [Header("UI Text References:")]
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI powerText;
    [SerializeField] TextMeshProUGUI priceBuyText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI totalBonusText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI saleLabelText;

    [Header("UI Interaction Elements:")]
    [SerializeField] Button buyButton;
    [SerializeField] Image workerImage;
    [SerializeField] Image backgroundImage;

    [Header("Visual Theme Settings:")]
    [SerializeField] Color normalColor = Color.white;
    [SerializeField] Color lockedColor = new Color(0.4f, 0.4f, 0.4f, 1f);

    System_Data data;
    Worker workerTemplate;
    int workerID;
    int cachedAmount;
    double cachedTotalPrice;
    bool lastKnownSaleState;
    bool isCurrentlyAffordable = true;

    void Update()
    {
        if (buyButton != null && data != null && workerTemplate != null)
        {
            if (lastKnownSaleState != data.isWorkerSaleActive)
            {
                lastKnownSaleState = data.isWorkerSaleActive;
                UpdatePriceText();
            }

            CalculateCurrentBulkStats();

            bool canAfford = data.pointsCounterFloat >= cachedTotalPrice && cachedAmount > 0;
            if (buyButton.interactable != canAfford)
            {
                buyButton.interactable = canAfford;
            }

            if (isCurrentlyAffordable != canAfford)
            {
                isCurrentlyAffordable = canAfford;
                ApplyVisualTheme(isCurrentlyAffordable);
            }

            UpdatePriceText();
        }
    }

    public void Setup(Worker worker, int id, System_Data dataRef)
    {
        workerTemplate = worker;
        workerID = id;
        data = dataRef;
        lastKnownSaleState = data.isWorkerSaleActive;

        UpdateUI();

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyWorker);
    }

    void CalculateCurrentBulkStats()
    {
        int currentLevel = data.workerLevels[workerID];
        float priceMod = data.isWorkerSaleActive ? 0.75f : 1.0f;

        switch (Worker_PurchaseSettings.CurrentMode)
        {
            case Worker_PurchaseSettings.PurchaseMode.x1:
                cachedAmount = 1;
                break;
            case Worker_PurchaseSettings.PurchaseMode.x5:
                cachedAmount = 5;
                break;
            case Worker_PurchaseSettings.PurchaseMode.Max:
                cachedAmount = workerTemplate.GetMaxAffordable(currentLevel, data.pointsCounterFloat, priceMod);
                if (cachedAmount <= 0) cachedAmount = 1;
                break;
        }

        cachedTotalPrice = workerTemplate.GetTotalCost(currentLevel, cachedAmount, priceMod);
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

        CalculateCurrentBulkStats();
        isCurrentlyAffordable = data.pointsCounterFloat >= cachedTotalPrice && cachedAmount > 0;
        ApplyVisualTheme(isCurrentlyAffordable);

        UpdatePriceText();
    }

    void ApplyVisualTheme(bool affordable)
    {
        Color applyColor = affordable ? normalColor : lockedColor;

        if (backgroundImage != null) backgroundImage.color = applyColor;
        if (workerImage != null) workerImage.color = applyColor;

        float textAlpha = affordable ? 1.0f : 0.5f;

        if (nameText != null) nameText.alpha = textAlpha;
        if (powerText != null) powerText.alpha = textAlpha;
        if (levelText != null) levelText.alpha = textAlpha;
        if (totalBonusText != null) totalBonusText.alpha = textAlpha;
        if (descriptionText != null) descriptionText.alpha = textAlpha;
    }

    void UpdatePriceText()
    {
        CalculateCurrentBulkStats();
        if (priceBuyText == null) return;

        if (saleLabelText != null)
            saleLabelText.gameObject.SetActive(data.isWorkerSaleActive);

        priceBuyText.color = data.isWorkerSaleActive ? Color.green : Color.white;

        string finalButtonText = "";

        switch (Worker_PurchaseSettings.CurrentMode)
        {
            case Worker_PurchaseSettings.PurchaseMode.x1:
                finalButtonText = $"Price x1\n{NumberFormatter.FormatWithDots(cachedTotalPrice)}";
                break;

            case Worker_PurchaseSettings.PurchaseMode.x5:
                finalButtonText = $"Price x5\n{NumberFormatter.FormatWithDots(cachedTotalPrice)}";
                break;

            case Worker_PurchaseSettings.PurchaseMode.Max:
                finalButtonText = $"MAX ({cachedAmount})\n{NumberFormatter.FormatWithDots(cachedTotalPrice)}";
                break;

            default:
                finalButtonText = NumberFormatter.FormatWithDots(cachedTotalPrice);
                break;
        }

        priceBuyText.text = finalButtonText;
    }
}