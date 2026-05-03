using TMPro;
using UnityEngine;

public class Prefab_Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;

    void Update()
    {
        if (timerText != null && Timer.Instance != null)
        {
            timerText.text = "Time \n" + Timer.Instance.GetFormattedTime();
        }
    }

    public void UpdateTimerPrefab() { }
}
