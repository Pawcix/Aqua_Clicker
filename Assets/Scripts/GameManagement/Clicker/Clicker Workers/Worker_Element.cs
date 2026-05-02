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

    void Update()
    {
        if (buyButton != null && data != null && workerTemplate != null)
        {
            double currentPrice = workerTemplate.GetPriceForLevel(data.workerLevels[workerID]);
            bool canAfford = data.pointsCounterFloat >= currentPrice;

            if (buyButton.interactable != canAfford)
            {
                buyButton.interactable = canAfford;
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

    void BuyWorker()
    {
        int currentLevel = data.workerLevels[workerID];
        double currentPrice = workerTemplate.GetPriceForLevel(currentLevel);

        if (data.pointsCounterFloat >= currentPrice)
        {
            data.pointsCounterFloat -= currentPrice;

            Clicker_System.OnItemBought.Invoke(currentPrice, workerTemplate.basePower);

            data.workerLevels[workerID]++;
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        if (data == null || workerTemplate == null) return;

        int currentLevel = data.workerLevels[workerID];
        double currentPrice = workerTemplate.GetPriceForLevel(currentLevel);

        float totalPPS = currentLevel * workerTemplate.basePower;

        nameText.text = workerTemplate.workerName;

        powerText.text = "+" + workerTemplate.basePower.ToString("F1") + "/s";

        priceBuyText.text = NumberFormatter.FormatWithDots(currentPrice);
        workerImage.sprite = workerTemplate.icon;

        if (levelText != null) levelText.text = "Level: " + currentLevel;

        if (totalBonusText != null) totalBonusText.text = "PPS: " + totalPPS.ToString("F1");

        if (descriptionText != null) descriptionText.text = workerTemplate.description;

        if (buyButton != null)
        {
            buyButton.interactable = data.pointsCounterFloat >= currentPrice;
        }
    }
}