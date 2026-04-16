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
        if (statsModal != null)
        {
            bool newState = !statsModal.activeSelf;
            statsModal.SetActive(newState);
        }
    }
}
