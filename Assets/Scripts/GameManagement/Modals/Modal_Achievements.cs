using UnityEngine;

public class Modal_Achievements : MonoBehaviour
{
    public GameObject achievementsModal;

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

        if (!wasActive)
        {
            achievementsModal.SetActive(true);
        }

        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Modal - Open and Close");
    }
}
