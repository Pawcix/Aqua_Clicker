using System.IO;
using UnityEngine;

public class Data_Reset : MonoBehaviour
{
    [Header("Settings:")]
    [SerializeField] System_Data systemData;
    [SerializeField] string fileName = "savegame.json";

    [ContextMenu("Reset Game Data")]
    public void ResetGame()
    {
        string savePath = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            // Debug.Log("<color=red><b>[Data Reset]</b></color> Save file deleted successfully.");
        }
        else
        {
            // Debug.Log("<color=yellow><b>[Data Reset]</b></color> No save file found to delete.");
        }

        if (systemData != null)
        {
            systemData.pointsCounter = 0;
            // Debug.Log("<color=cyan><b>[Data Reset]</b></color> System data counters reset to 0.");
        }
        else
        {
            // Debug.LogWarning("[Data Reset] System_Data reference missing! Counters not reset.");
        }
    }
}
