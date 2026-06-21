using UnityEngine;

public class Modal_Settings : MonoBehaviour
{
    public GameObject settingsModal;

    void Awake()
    {
        if (settingsModal != null)
        {
            settingsModal.SetActive(false);
        }
    }

    public void ToggleSettings()
    {
        if (settingsModal == null) return;

        bool wasActive = settingsModal.activeInHierarchy;

        if (!wasActive)
        {
            settingsModal.SetActive(true);
        }

        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Modal - Open and Close");
    }
}
