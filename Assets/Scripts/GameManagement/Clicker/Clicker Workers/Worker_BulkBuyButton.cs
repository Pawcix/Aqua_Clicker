using TMPro;
using UnityEngine;

public class Worker_BulkBuyButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI buttonText;

    public void ToggleMode()
    {
        Worker_PurchaseSettings.CycleMode();

        if (buttonText != null)
            buttonText.text = "BUY: " + Worker_PurchaseSettings.CurrentMode.ToString();

        Worker_List workerList = Object.FindAnyObjectByType<Worker_List>();

        if (workerList != null)
        {
            workerList.RefreshAllElements();
        }
    }
}