using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class Data_SaveManager : MonoBehaviour
{
    public static Data_SaveManager instance;

    [SerializeField] System_Data systemData;
    [SerializeField] Clicker_Skills clickerSkills;

    string savePath;

    void Awake()
    {
        instance = this;
        savePath = Path.Combine(Application.persistentDataPath, "savegame.json");
        LoadGame();
    }

    public void SaveGame()
    {
        GameData dataToSave = new GameData
        {
            score = systemData.pointsCounter,
            pps = systemData.pointsPerSecond,
            workerLevels = new List<int>(systemData.workerLevels),
            time = Timer.Instance != null ? Timer.Instance.TotalPlayTime : systemData.timer,
            skinIndex = systemData.currentSkinIndex,
            autoClickActive = systemData.isAutoClickerActive
        };

        string json = JsonUtility.ToJson(dataToSave, true);
        File.WriteAllText(savePath, json);
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath)) return;

        string json = File.ReadAllText(savePath);
        GameData loadedData = JsonUtility.FromJson<GameData>(json);

        systemData.pointsCounter = loadedData.score;
        systemData.pointsPerSecond = loadedData.pps;
        systemData.timer = loadedData.time;
        systemData.currentSkinIndex = loadedData.skinIndex;
        systemData.isAutoClickerActive = loadedData.autoClickActive;

        if (loadedData.workerLevels != null)
        {
            systemData.workerLevels = new List<int>(loadedData.workerLevels);
        }

        if (Timer.Instance != null) Timer.Instance.LoadSavedTime(loadedData.time);
        if (clickerSkills != null) clickerSkills.RefreshSkillsVisuals();
    }

    void OnApplicationQuit() => SaveGame();
}
