using UnityEngine;

public class Modal_Rebirth : MonoBehaviour
{
    public GameObject rebirthModal;
    public KeyShorts keyShortsSource;

    void Awake()
    {
        if (rebirthModal != null)
        {
            rebirthModal.SetActive(false);
        }
    }

    public void ToggleSettings()
    {
        if (rebirthModal == null) return;

        bool wasActive = rebirthModal.activeInHierarchy;

        if (keyShortsSource != null)
        {
            keyShortsSource.CloseAllModals();
        }

        if (!wasActive)
        {
            rebirthModal.SetActive(true);
        }

        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Modal - Open and Close");
    }
}
