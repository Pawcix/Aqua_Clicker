using UnityEngine;

public class Modal_Skills : MonoBehaviour
{
    public GameObject skillsModal;
    public KeyShorts keyShortsSource;

    void Awake()
    {
        if (skillsModal != null)
        {
            skillsModal.SetActive(false);
        }
    }

    public void ToggleSettings()
    {
        if (skillsModal == null) return;

        bool wasActive = skillsModal.activeInHierarchy;

        if (keyShortsSource != null)
        {
            keyShortsSource.CloseAllModals();
        }

        if (!wasActive)
        {
            skillsModal.SetActive(true);
        }
    }
}
