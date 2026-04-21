using UnityEngine;

public class Modal_DebugPanel : MonoBehaviour
{
    public GameObject debugPanelModal;

    void Awake()
    {
        if (debugPanelModal != null)
        {
            debugPanelModal.SetActive(false);
        }
    }

    public void ToggleSettings()
    {
        if (debugPanelModal != null)
        {
            bool isVisible = debugPanelModal.activeSelf;
            debugPanelModal.SetActive(!isVisible);

            // Debug.Log($"[debugPanelModal] Modal jest teraz: {(!isVisible ? "WŁĄCZONY" : "WYŁĄCZONY")}");
        }
    }
}
