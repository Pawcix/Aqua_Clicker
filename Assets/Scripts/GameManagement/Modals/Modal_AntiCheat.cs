using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Modal_AntiCheat : MonoBehaviour
{
    [Header("UI Elements:")]
    [SerializeField] GameObject antiCheatModal;
    [SerializeField] TextMeshProUGUI timerText;

    [Header("Settings:")]
    [SerializeField] int penaltyTime = 5;


    void Awake()
    {
        if (antiCheatModal != null)
        {
            antiCheatModal.SetActive(false);
        }
    }

    public void ShowModal()
    {
        if (antiCheatModal != null)
        {
            antiCheatModal.SetActive(true);

            StopAllCoroutines();
            StartCoroutine(AutoPenaltyCoroutine());
        }
    }

    IEnumerator AutoPenaltyCoroutine()
    {
        int currentTime = penaltyTime;

        while (currentTime > 0)
        {
            if (timerText != null)
                timerText.text = $"Wait {currentTime}s...";

            yield return new WaitForSecondsRealtime(1f);
            currentTime--;
        }

        // KONIEC KARY
        if (timerText != null) timerText.text = "Returning to game...";
        yield return new WaitForSecondsRealtime(0.5f);

        CloseModalAutomatically();
    }

    void CloseModalAutomatically()
    {
        if (antiCheatModal != null)
        {
            antiCheatModal.SetActive(false);

            System_AntiCheat antiCheatSystem = Object.FindFirstObjectByType<System_AntiCheat>();
            if (antiCheatSystem != null)
            {
                antiCheatSystem.ResetModalState();
            }
        }
    }
}
