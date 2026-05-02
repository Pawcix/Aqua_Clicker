using UnityEngine;

public class Modal_History : MonoBehaviour
{
    public GameObject historyModal;
    public KeyShorts keyShortsSource;

    void Awake()
    {
        if (historyModal != null)
        {
            historyModal.SetActive(false);
        }
    }

    public void ToggleSettings()
    {
        if (historyModal == null) return;

        bool wasActive = historyModal.activeInHierarchy;

        if (keyShortsSource != null)
        {
            keyShortsSource.CloseAllModals();
        }

        if (!wasActive)
        {
            historyModal.SetActive(true);
        }
    }
}
