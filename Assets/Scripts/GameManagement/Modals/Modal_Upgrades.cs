using UnityEngine;

public class Modal_Upgrades : MonoBehaviour
{
    public GameObject upgradesModal;
    public KeyShorts keyShortsSource;

    void Awake()
    {
        if (upgradesModal != null)
        {
            upgradesModal.SetActive(false);
        }
    }

    public void ToggleSettings()
    {
        if (upgradesModal == null) return;

        bool wasActive = upgradesModal.activeInHierarchy;

        if (keyShortsSource != null)
        {
            keyShortsSource.CloseAllModals();
        }

        if (!wasActive)
        {
            upgradesModal.SetActive(true);
        }
    }
}
