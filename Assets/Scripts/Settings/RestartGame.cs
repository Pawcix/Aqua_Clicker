using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    [Header("Scene Loading Settings:")]
    [SerializeField] string loadingSceneName = "Scene_Loading";

    [Header("Data Source Link:")]
    [SerializeField] System_Data data;

    bool isRestarting = false;

    public void RestartGameOnButton()
    {
        if (isRestarting) return;
        isRestarting = true;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("Main Button");
        }

        if (data != null)
        {
            data.pointsCounterFloat = 0;
            data.highestCritMultiplier = 1.0;
        }

        SceneManager.LoadScene(loadingSceneName);
    }
}