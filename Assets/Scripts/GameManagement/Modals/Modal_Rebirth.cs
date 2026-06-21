using UnityEngine;

public class Modal_Rebirth : MonoBehaviour
{
    public GameObject rebirthModal;

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

        if (!wasActive)
        {
            rebirthModal.SetActive(true);
        }

        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Modal - Open and Close");
    }
}
