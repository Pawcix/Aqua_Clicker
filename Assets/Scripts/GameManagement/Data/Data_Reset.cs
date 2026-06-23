using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Data_Reset : MonoBehaviour
{
    [Header("Data Source:")]
    [SerializeField] System_Data systemData;
    [SerializeField] string fileName = "savegame.dat";

    [Header("Scene Loading Settings:")]
    [SerializeField] string loadingSceneName = "Scene_Loading";

    bool isResetting = false;

    public void ResetGame()
    {
        if (isResetting) return;
        isResetting = true;

        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Main Button");
        if (System_Achievements.Instance != null) System_Achievements.Instance.DisableChecking();

        string savePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(savePath)) File.Delete(savePath);

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        if (systemData != null)
        {
            systemData.pointsCounterFloat = 0f;
            systemData.goldenDrops = 0;
            systemData.basePPS = 0;
            systemData.workersPPS = 0;
            systemData.adMultiplier = 1.0f;
            if (systemData.unlockedAchievementIDs != null) systemData.unlockedAchievementIDs.Clear();
        }

        IrisMaskController transition = Object.FindFirstObjectByType<IrisMaskController>();
        if (transition != null)
        {
            transition.targetScene = loadingSceneName;
            transition.StartFadeOut();
        }
        else
        {
            SceneManager.LoadScene(loadingSceneName);
        }
    }
}