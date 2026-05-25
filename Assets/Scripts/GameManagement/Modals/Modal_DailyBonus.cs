using UnityEngine;

public class Modal_DailyBonus : MonoBehaviour
{
    [SerializeField] private System_Data data;
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

        bool wasActive = dailyBonusModal.activeSelf;

        if (keyShortsSource != null)
        {
            keyShortsSource.CloseAllModals();
        }

        if (!wasActive)
        {
            dailyBonusModal.SetActive(true);
        }
        else
        {
            dailyBonusModal.SetActive(false);
        }
    }
}