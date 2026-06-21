using UnityEngine;

public class Modal_WheelReward : MonoBehaviour
{
    public GameObject wheelRewardModal;

    void Awake()
    {
        if (wheelRewardModal != null)
        {
            wheelRewardModal.SetActive(false);
        }
    }

    public void ToggleSettings()
    {
        if (wheelRewardModal == null) return;

        bool wasActive = wheelRewardModal.activeInHierarchy;

        if (!wasActive)
        {
            wheelRewardModal.SetActive(true);
        }
        else
        {
            if (System_NotificationWheelReward.Instance != null)
            {
                System_NotificationWheelReward.Instance.SetAlert(false);
            }
        }

        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Modal - Open and Close");
    }
}