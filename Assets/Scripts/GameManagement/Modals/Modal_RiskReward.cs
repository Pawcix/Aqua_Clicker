using UnityEngine;

public class Modal_RiskReward : MonoBehaviour
{
    public GameObject riskRewardModal;
    public KeyShorts keyShortsSource;

    [SerializeField] private System_RiskReward systemRiskReward;

    [Header("DEBUGER:")]
    [SerializeField] private bool forceOpenInEditor = true; 
    
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
        if (riskRewardModal == null || systemRiskReward == null) return;

        bool wasActive = riskRewardModal.activeInHierarchy;

        if (wasActive)
        {
            riskRewardModal.SetActive(false);
            return;
        }

        if (forceOpenInEditor || systemRiskReward.CanPlayerPlay())
        {
            if (keyShortsSource != null)
            {
                keyShortsSource.CloseAllModals();
            }

            riskRewardModal.SetActive(true);

            systemRiskReward.OpenRiskWindow();
        }
    }
}