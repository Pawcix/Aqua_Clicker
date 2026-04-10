using UnityEngine;

public class Exit : MonoBehaviour
{
    public GameObject exitPanel;

    void Awake()
    {
        if (exitPanel != null)
        {
            exitPanel.SetActive(false);
        }
    }

    public void ToggleExit()
    {
        if (exitPanel != null)
        {
            bool newState = !exitPanel.activeSelf;
            exitPanel.SetActive(newState);
        }
    }

    public void ExitGameOnButton()
    {
        // Debug.Log("Exiting game...");
        Application.Quit();
    }
}
