using UnityEngine;

public class Modal_Wardrobe : MonoBehaviour
{
    public GameObject wardrobeModal;
    public KeyShorts keyShortsSource;

    void Awake()
    {
        if (wardrobeModal != null)
        {
            wardrobeModal.SetActive(false);
        }
    }

    public void ToggleSettings()
    {
        if (wardrobeModal == null) return;

        bool wasActive = wardrobeModal.activeInHierarchy;

        if (keyShortsSource != null)
        {
            keyShortsSource.CloseAllModals();
        }

        if (!wasActive)
        {
            wardrobeModal.SetActive(true);
            // if (System_Notification.Instance != null)
            // {
            //     System_Notification.Instance.SetAlert(false);
            // }
        }
        else
        {
            wardrobeModal.SetActive(false);
        }
    }

    public void OnWardrobeButtonClicked()
    {
        // if (System_Notification.Instance != null)
        // {
        //     System_Notification.Instance.SetAlert(false);
        // }
    }
}
