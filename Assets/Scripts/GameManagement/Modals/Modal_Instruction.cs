using UnityEngine;

public class Modal_Instruction : MonoBehaviour
{
    public GameObject instructionModal;
    public KeyShorts keyShortsSource;

    void Awake()
    {
        if (instructionModal != null)
        {
            instructionModal.SetActive(false);
        }
    }

    public void ToggleSettings()
    {
        if (instructionModal == null) return;

        bool wasActive = instructionModal.activeInHierarchy;

        if (keyShortsSource != null)
        {
            keyShortsSource.CloseAllModals();
        }

        if (!wasActive)
        {
            instructionModal.SetActive(true);
        }
    }
}
