using UnityEngine;

public class Modal_Settings : MonoBehaviour
{
    public GameObject settingsModal;

    void Awake()
    {
        if (settingsModal != null)
        {
            settingsModal.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettings();
        }
    }

    public void ToggleSettings()
    {
        if (settingsModal != null)
        {
            bool newState = !settingsModal.activeSelf;
            settingsModal.SetActive(newState);
        }
    }
}
