using UnityEngine;

public class Modal_Stats : MonoBehaviour
{
    public GameObject statsModal;
    public KeyShorts keyShortsSource;

    void Awake()
    {
        if (statsModal != null)
        {
            statsModal.SetActive(false);
        }
    }

    public void ToggleSettings()
    {
        if (statsModal == null) return;

        bool wasActive = statsModal.activeInHierarchy;

        if (keyShortsSource != null)
        {
            keyShortsSource.CloseAllModals();
        }

        if (!wasActive)
        {
            statsModal.SetActive(true);
        }
    }
}
