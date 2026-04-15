using UnityEngine;

public class Modal_VideoSettings : MonoBehaviour
{
    public GameObject videoSettingsModal;

    void Awake()
    {
        if (videoSettingsModal != null)
        {
            videoSettingsModal.SetActive(false);
        }
    }

    public void ToggleExit()
    {
        if (videoSettingsModal != null)
        {
            bool newState = !videoSettingsModal.activeSelf;
            videoSettingsModal.SetActive(newState);
        }
    }
}
