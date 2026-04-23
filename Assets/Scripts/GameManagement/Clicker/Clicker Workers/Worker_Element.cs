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
        if (data.pointsCounter >= workerTemplate.basePrice)
        {
            Clicker_System.OnItemBought.Invoke(workerTemplate.basePrice, workerTemplate.basePower);

            data.workerLevels[workerID]++;
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        int currentLevel = data.workerLevels[workerID];
        int totalPPS = currentLevel * workerTemplate.basePower;

        nameText.text = workerTemplate.workerName;
        powerText.text = "+" + workerTemplate.basePower + "/s";
        priceBuyText.text = workerTemplate.basePrice.ToString();
        workerImage.sprite = workerTemplate.icon;
        descriptionText.text = workerTemplate.description;

        if (levelText != null) levelText.text = "Level: " + currentLevel;
        if (totalBonusText != null) totalBonusText.text = "PPS: " + totalPPS;
        if (descriptionText != null) descriptionText.text = workerTemplate.description;
    }
}