using UnityEngine;

public class Modal_Skills : MonoBehaviour
{
    public GameObject skillsModal;

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

        if (!wasActive)
        {
            skillsModal.SetActive(true);
        }
    }
}
