using UnityEngine;

public class Modal : MonoBehaviour
{
    public GameObject modalPanel;

    public void ToggleModalSettings()
    {
        if (modalPanel != null)
        {
            bool newState = !modalPanel.activeSelf;
            modalPanel.SetActive(newState);
            AudioManager.Instance.PlaySFX("Open");
            // Debug.Log($"[Modal] - {modalPanel.name} - is now: {(newState ? "ENABLED :)" : "DISABLED :(")}");
        }
        else
        {
            // Debug.LogWarning("[Modal] Error: modalPanel is not assigned in the Inspector!");
        }
    }
}