using TMPro;
using UnityEngine;

public class AntiCheat_Break : MonoBehaviour
{
    [Header("Settings:")]
    [SerializeField] float reminderIntervalMinutes = 120f;

    [Header("UI References:")]
    [SerializeField] GameObject healthModal;
    [SerializeField] TextMeshProUGUI sessionTimeText;

    private float sessionTimer = 0f;
    private bool wasShownThisSession = false;

    void Start()
    {
        if (healthModal != null)
            healthModal.SetActive(false);

        sessionTimer = 0f;
    }

    void Update()
    {
        if (!wasShownThisSession)
        {
            sessionTimer += Time.deltaTime;

            if (sessionTimer >= reminderIntervalMinutes * 60f)
            {
                ShowHealthReminder();
            }
        }
    }

    void ShowHealthReminder()
    {
        Debug.Log("[Health Check] Aktywowano przypomnienie o przerwie.");

        if (Modal_AC_Break.Instance != null)
        {
            Modal_AC_Break.Instance.ShowModal();
        }
        else
        {
            Debug.LogError("Nie znaleziono Modal_AC_Break na scenie! Sprawdź czy skrypt jest przypisany do obiektu.");
        }
    }

    public void CloseModal()
    {
        if (healthModal != null)
        {
            healthModal.SetActive(false);
        }
    }
}
