using UnityEngine;

public class Modal_Wardrobe : MonoBehaviour
{
    public GameObject wardrobeModal;

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
            if (System_Notification.Instance != null)
            {
                System_Notification.Instance.SetAlert(false);
            }
        }

        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Open");
    }
}

