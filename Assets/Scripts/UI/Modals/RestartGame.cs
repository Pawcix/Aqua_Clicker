using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public GameObject restartGamePanel;

    void Awake()
    {
        if (restartGamePanel != null)
        {
            restartGamePanel.SetActive(false);
        }
    }

    public void ToggleSettings()
    {
        if (restartGamePanel != null)
        {
            bool newState = !restartGamePanel.activeSelf;
            restartGamePanel.SetActive(newState);
        }
    }

    public void RestartGameOnButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        AudioManager.Instance.PlayMusic("Theme");
    }
}
