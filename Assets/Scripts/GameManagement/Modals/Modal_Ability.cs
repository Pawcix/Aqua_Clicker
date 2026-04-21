using UnityEngine;

public class Modal_Ability : MonoBehaviour
{
    public GameObject abilityModal;
    public KeyShorts keyShortsSource;

    void Awake()
    {
        if (abilityModal != null)
        {
            abilityModal.SetActive(false);
        }
    }

    public void ToggleSettings()
    {
        if (abilityModal == null) return;

        bool wasActive = abilityModal.activeInHierarchy;

        if (keyShortsSource != null)
        {
            keyShortsSource.CloseAllModals();
        }

        if (!wasActive)
        {
            abilityModal.SetActive(true);
        }
    }
}
