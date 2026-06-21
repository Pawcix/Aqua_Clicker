using UnityEngine;

public class Modal_Stats : MonoBehaviour
{
    public GameObject statsModal;

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

        if (!wasActive)
        {
            statsModal.SetActive(true);
        }
    }
}
