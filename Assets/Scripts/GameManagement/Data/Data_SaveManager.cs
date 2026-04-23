using System.IO;
using UnityEngine;

public class Data_SaveManager : MonoBehaviour
{
    public static Data_SaveManager instance;

    [SerializeField] System_Data systemData;
    string savePath;
    const string fileName = "savegame.json";

    void Awake()
    {
        instance = this;
        savePath = Path.Combine(Application.persistentDataPath, "savegame.json");

        LoadGame();
    }

    public void SaveGame()
    {
        GameData dataToSave = new GameData { score = systemData.pointsCounter };
        string json = JsonUtility.ToJson(dataToSave, true);
        File.WriteAllText(savePath, json);
        // Debug.Log($"<color=green><b>[Save System]</b></color> Game Saved. Score: {dataToSave.score}");
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            GameData loadedData = JsonUtility.FromJson<GameData>(json);

            systemData.pointsCounter = loadedData.score;
            // Debug.Log($"<color=cyan><b>[Save System]</b></color> Game Loaded. Score: {loadedData.score}");
        }
        else
        {
            // Debug.Log("<color=orange><b>[Save System]</b></color> No save file found. Starting fresh.");
        }
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }
}
