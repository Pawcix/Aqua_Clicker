using UnityEngine;

public class Modal_Upgrades : MonoBehaviour
{
    public GameObject upgradesModal;

    void Awake()
    {
        if (upgradesModal != null)
        {
            upgradesModal.SetActive(false);
        }
    }

    public void ToggleSettings()
    {
        if (upgradesModal == null) return;

        bool wasActive = upgradesModal.activeInHierarchy;

        if (!wasActive)
        {
            upgradesModal.SetActive(true);
        }
    }
}
