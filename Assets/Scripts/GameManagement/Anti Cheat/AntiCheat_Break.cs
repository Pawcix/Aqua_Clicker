using TMPro;
using UnityEngine;

public class AntiCheat_Break : MonoBehaviour
{
    [Header("Settings:")]
    [SerializeField] float reminderIntervalMinutes = 120f;

    [Header("UI References:")]
    [SerializeField] GameObject healthModal;

    float sessionTimer = 0f;
    bool wasShownThisSession = false;

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
        if (Modal_AC_Break.Instance != null)
        {
            int hours = (int)(sessionTimer / 3600);
            int minutes = (int)((sessionTimer % 3600) / 60);
            int seconds = (int)(sessionTimer % 60);
            string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);

            Modal_AC_Break.Instance.ShowModal(formattedTime);
            wasShownThisSession = true;
        }
    }
}
