using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Data_SaveManager : MonoBehaviour
{
    public static Data_SaveManager instance;

    [SerializeField] System_Data systemData;
    [SerializeField] Clicker_Skills clickerSkills;

    string savePath;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        savePath = Path.Combine(Application.persistentDataPath, "savegame.json");
        LoadGame();
    }

    public void SaveGame()
    {
        if (systemData == null) return;

        GameData dataToSave = new GameData
        {
            score = systemData.pointsCounter,
            pps = systemData.pointsPerSecond,
            workerLevels = new List<int>(systemData.workerLevels),
            time = Timer.Instance != null ? Timer.Instance.TotalPlayTime : systemData.timer,
            skinIndex = systemData.currentSkinIndex,
            autoClickActive = systemData.isAutoClickerActive,
            antiCheatBypassActive = systemData.isAntiCheatBypassActive
        };

        dataToSave.totalAwayEarnings = systemData.totalAwayEarnings;

        string json = JsonUtility.ToJson(dataToSave, true);
        File.WriteAllText(savePath, json);
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath)) return;

        string json = File.ReadAllText(savePath);
        GameData loadedData = JsonUtility.FromJson<GameData>(json);

        if (loadedData == null) return;

        systemData.pointsCounter = loadedData.score;
        systemData.pointsPerSecond = loadedData.pps;
        systemData.timer = loadedData.time;
        systemData.currentSkinIndex = loadedData.skinIndex;
        systemData.isAntiCheatBypassActive = loadedData.antiCheatBypassActive;
        systemData.isAutoClickerActive = loadedData.autoClickActive;

        if (loadedData.workerLevels != null)
        {
            systemData.workerLevels = new List<int>(loadedData.workerLevels);
        }

        systemData.totalAwayEarnings = loadedData.totalAwayEarnings;

        if (Timer.Instance != null) Timer.Instance.LoadSavedTime(loadedData.time);
        if (clickerSkills != null) clickerSkills.RefreshSkillsVisuals();
    }

    public void SaveLastSeenTime()
    {
        PlayerPrefs.SetString("LastSeen", DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    void OnApplicationQuit()
    {
        SaveLastSeenTime();
        SaveGame();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveLastSeenTime();
            SaveGame();
        }
        else
        {
            var awaySystem = UnityEngine.Object.FindFirstObjectByType<System_AwayIncome>();
            if (awaySystem != null)
            {
                awaySystem.CalculateAwayIncome();
            }
        }
    }
}
