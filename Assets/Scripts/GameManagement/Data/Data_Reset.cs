using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Data_Reset : MonoBehaviour
{
    [SerializeField] System_Data systemData;
    [SerializeField] string fileName = "savegame.json";
    [ContextMenu("Reset Game Data")]

    public void ResetGame()
    {
        string savePath = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("<color=red><b>[Data Reset]</b></color> Plik zapisu został usunięty.");
        }

        if (systemData != null)
        {
            systemData.pointsCounterFloat = 0f;
            systemData.timer = 0f;
            systemData.goldenDrops = 0;

            // Debug.Log("<color=cyan><b>[Data Reset]</b></color> Punkty i liczniki zostały wyzerowane.");

            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
        else
        {
            // Debug.LogWarning("[Data Reset] Brak referencji do System_Data!");
        }
    }
}
