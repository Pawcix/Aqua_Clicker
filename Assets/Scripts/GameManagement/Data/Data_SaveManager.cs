using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class Data_SaveManager : MonoBehaviour
{
    public static Data_SaveManager instance;

    [SerializeField] System_Data systemData;
    [SerializeField] Clicker_Skills clickerSkills;

    readonly string encryptionKey = "BU+hj{a^6ScB*egWYnpqqaNz=-rC[s6^L9MHVx,3";
    string savePath;

    void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }

        savePath = Path.Combine(Application.persistentDataPath, "savegame.dat");
        LoadGame();
    }

    public void SaveGame()
    {
        if (systemData == null) return;

        GameData dataToSave = new GameData
        {
            score = systemData.pointsCounterFloat,
            pps = systemData.pointsPerSecond,
            workerLevels = new List<int>(systemData.workerLevels),
            unlockedSkinIDs = new List<int>(systemData.unlockedSkinIDs),
            time = Timer.Instance != null ? Timer.Instance.TotalPlayTime : systemData.timer,
            skinIndex = systemData.currentSkinIndex,
            clickMultiplier = systemData.clickMultiplier,
            autoClickActive = systemData.isAutoClickerActive,
            antiCheatBypassActive = systemData.isAntiCheatBypassActive,
            goldenDrops = systemData.goldenDrops,
            goldenRainTimer = systemData.goldenRainTimer,
            luckyBonus = systemData.luckyBonus,
            highestComboMultiplier = systemData.highestComboMultiplier,
            autoCollectorActive = systemData.isAutoCollectorActive,
            LuckyCollectorActive = systemData.isLuckyCollectorActive,
            totalAwayEarnings = systemData.totalAwayEarnings,
        };

        string json = JsonUtility.ToJson(dataToSave, true);
        string encryptedJson = EncryptDecrypt(json);

        File.WriteAllText(savePath, encryptedJson);
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath)) return;

        try
        {
            string encryptedJson = File.ReadAllText(savePath);
            string json = EncryptDecrypt(encryptedJson);

            GameData loadedData = JsonUtility.FromJson<GameData>(json);
            if (loadedData == null) throw new Exception("Invalid Data");

            systemData.pointsCounterFloat = loadedData.score;
            systemData.totalAwayEarnings = loadedData.totalAwayEarnings;
            systemData.pointsPerSecond = loadedData.pps;
            systemData.timer = loadedData.time;
            systemData.currentSkinIndex = loadedData.skinIndex;
            systemData.clickMultiplier = loadedData.clickMultiplier;
            systemData.isAntiCheatBypassActive = loadedData.antiCheatBypassActive;
            systemData.isAutoClickerActive = loadedData.autoClickActive;
            systemData.goldenDrops = loadedData.goldenDrops;
            systemData.luckyBonus = loadedData.luckyBonus;
            systemData.goldenRainTimer = loadedData.goldenRainTimer;
            systemData.isAutoCollectorActive = loadedData.autoCollectorActive;
            systemData.isLuckyCollectorActive = loadedData.LuckyCollectorActive;
            systemData.highestComboMultiplier = loadedData.highestComboMultiplier;
            
            if (loadedData.workerLevels != null)
                systemData.workerLevels = new List<int>(loadedData.workerLevels);

            if (loadedData.unlockedSkinIDs != null)
                systemData.unlockedSkinIDs = new List<int>(loadedData.unlockedSkinIDs);

            if (Timer.Instance != null) Timer.Instance.LoadSavedTime(loadedData.time);
            if (clickerSkills != null) clickerSkills.RefreshSkillsVisuals();

            if (System_Wardrobe.Instance != null)
                System_Wardrobe.Instance.LoadSkin(systemData.currentSkinIndex);
        }
        catch (Exception e)
        {
            Debug.LogError("Wykryto zmianę w pliku zapisu lub błąd konwersji: " + e.Message);

            if (AntiCheat_JSON.Instance != null)
            {
                AntiCheat_JSON.Instance.TriggerFileTamperProtection();
            }
        }
    }

    string EncryptDecrypt(string data)
    {
        StringBuilder result = new StringBuilder();
        for (int i = 0; i < data.Length; i++)
        {
            result.Append((char)(data[i] ^ encryptionKey[i % encryptionKey.Length]));
        }
        return result.ToString();
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
            var away = UnityEngine.Object.FindFirstObjectByType<System_AwayIncome>();
            if (away != null) away.CalculateAwayIncome();
        }
    }

    // [ContextMenu("Eksportuj zapis do TXT na Pulpit")]
    // public void ExportToDesktop()
    // {
    //     string path = Path.Combine(Application.persistentDataPath, "savegame.dat");
    //     if (File.Exists(path))
    //     {
    //         string decrypted = EncryptDecrypt(File.ReadAllText(path));
    //         string desktopPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "DEKODER_AQUA.txt");
    //         File.WriteAllText(desktopPath, decrypted);
    //     }
    // }
}
