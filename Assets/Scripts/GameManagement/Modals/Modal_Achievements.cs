using UnityEngine;

public class Modal_Achievements : MonoBehaviour
{
    public GameObject achievementsModal;
    public KeyShorts keyShortsSource;

    void Awake()
    {
        if (achievementsModal != null)
        {
            achievementsModal.SetActive(false);
        }
    }

    public void ToggleSettings()
    {
        if (achievementsModal == null) return;

        bool wasActive = achievementsModal.activeInHierarchy;

        if (keyShortsSource != null)
        {
            keyShortsSource.CloseAllModals();
        }

        if (!wasActive)
        {
            achievementsModal.SetActive(true);
        }
    }
}
