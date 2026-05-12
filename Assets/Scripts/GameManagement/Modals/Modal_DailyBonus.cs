using UnityEngine;

public class Modal_DailyBonus : MonoBehaviour
{
    [SerializeField] System_Data data;
    public GameObject dailyBonusModal;
    public GameObject openButton;
    public KeyShorts keyShortsSource;

    void Awake()
    {
        if (dailyBonusModal != null)
        {
            dailyBonusModal.SetActive(false);
        }
    }

    void Update()
    {
        UpdateAvailableState();
    }

    public void UpdateAvailableState()
    {
        if (data == null || openButton == null) return;

        openButton.SetActive(data.loginStreak > 0);
    }

    public void ToggleSettings()
    {
        if (dailyBonusModal == null) return;

        bool wasActive = dailyBonusModal.activeInHierarchy;

        if (wasActive)
        {
            dailyBonusModal.SetActive(false);
        }
        else
        {
            if (keyShortsSource != null) keyShortsSource.CloseAllModals();

            dailyBonusModal.SetActive(true);

            if (System_NotificationDailyBonus.Instance != null)
            {
                System_NotificationDailyBonus.Instance.SetAlert(false);
            }

            if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Open");
        }
    }
}