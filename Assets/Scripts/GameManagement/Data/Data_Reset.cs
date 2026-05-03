using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Data_Reset : MonoBehaviour
{
    [SerializeField] System_Data systemData;
    [SerializeField] string fileName = "savegame.json";

    public void ResetGame()
    {
        if (System_Achievements.Instance != null)
            System_Achievements.Instance.DisableChecking();

        string savePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(savePath)) File.Delete(savePath);

        PlayerPrefs.DeleteAll();

        if (systemData != null)
        {
            systemData.pointsCounterFloat = 0f;
            systemData.goldenDrops = 0;
            systemData.unlockedAchievementIDs.Clear();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}