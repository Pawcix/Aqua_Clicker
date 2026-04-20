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
        if (skillsModal != null)
        {
            bool isVisible = skillsModal.activeSelf;
            skillsModal.SetActive(!isVisible);

            // Debug.Log($"[Modal_Skills] Modal jest teraz: {(!isVisible ? "WŁĄCZONY" : "WYŁĄCZONY")}");
        }
    }
}
