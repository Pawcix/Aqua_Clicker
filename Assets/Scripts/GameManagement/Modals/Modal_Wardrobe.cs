using UnityEngine;

public class Modal_Wardrobe : MonoBehaviour
{
    public GameObject wardrobeModal;
    public KeyShorts keyShortsSource;

    void Awake()
    {
        if (wardrobeModal != null) wardrobeModal.SetActive(false);
    }

    public void ToggleSettings()
    {
        if (wardrobeModal == null) return;

        bool wasActive = wardrobeModal.activeInHierarchy;

        if (!wasActive)
        {
            wardrobeModal.SetActive(true);
        }

        else
        {
            if (keyShortsSource != null) keyShortsSource.CloseAllModals();

            wardrobeModal.SetActive(true);

            if (System_Notification.Instance != null)
            {
                System_Notification.Instance.SetAlert(false);
            }

            if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Open");
        }
    }
}

