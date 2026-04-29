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
        }
    }
}
