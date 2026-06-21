using UnityEngine;

public class Modal_Ability : MonoBehaviour
{
    public GameObject abilityModal;

    void Awake()
    {
        if (abilityModal != null)
        {
            abilityModal.SetActive(false);
        }
    }

    public void ToggleSettings()
    {
        if (abilityModal == null) return;

        bool wasActive = abilityModal.activeInHierarchy;

        if (!wasActive)
        {
            abilityModal.SetActive(true);
        }
    }
}
