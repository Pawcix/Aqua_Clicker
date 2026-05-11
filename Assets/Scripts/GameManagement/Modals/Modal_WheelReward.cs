using UnityEngine;

public class Modal_WheelReward : MonoBehaviour
{
    public GameObject wheelRewardModal;
    public KeyShorts keyShortsSource;

    void Awake()
    {
        if (wheelRewardModal != null) wheelRewardModal.SetActive(false);
    }

    public void ToggleSettings()
    {
        if (wheelRewardModal == null) return;

        bool wasActive = wheelRewardModal.activeInHierarchy;

        if (wasActive)
        {
            wheelRewardModal.SetActive(false);
        }

        else
        {
            if (keyShortsSource != null) keyShortsSource.CloseAllModals();

            wheelRewardModal.SetActive(true);

            if (System_Notification.Instance != null)
            {
                System_Notification.Instance.SetAlert(false);
            }

            if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Open");
        }
    }
}
