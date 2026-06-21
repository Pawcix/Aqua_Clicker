using UnityEngine;

public class Modal_RiskReward : MonoBehaviour
{
    public GameObject riskRewardModal;

    [SerializeField] System_RiskReward systemRiskReward;

    void Awake()
    {
        if (riskRewardModal != null)
        {
            riskRewardModal.SetActive(false);
        }

        if (systemRiskReward == null)
        {
            systemRiskReward = FindFirstObjectByType<System_RiskReward>();
        }
    }

    public void ToggleSettings()
    {
        if (riskRewardModal == null) return;

        bool wasActive = riskRewardModal.activeInHierarchy;

        if (!wasActive)
        {
            riskRewardModal.SetActive(true);
        }
        else
        {
            if (System_Notification.Instance != null)
            {
                System_Notification.Instance.SetAlert(false);
            }
        }

        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Modal - Open and Close");

        if (systemRiskReward.CanPlayerPlay())
        {
            systemRiskReward.OpenRiskWindow();
        }
    }
}