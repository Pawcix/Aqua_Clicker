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

    void Start()
    {
        InvokeRepeating(nameof(TriggerAutoSave), 10f, 30f);
    }

    void TriggerAutoSave()
    {
        SaveLastSeenTime();
        SaveGame();
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
            seenSkinIDs = new List<int>(systemData.seenSkinIDs),
            time = Timer.Instance != null ? Timer.Instance.TotalPlayTime : systemData.timer,
            skinIndex = systemData.currentSkinIndex,
            clickMultiplier = systemData.clickMultiplier,
            autoClickActive = systemData.isAutoClickerActive,
            antiCheatBypassActive = systemData.isAntiCheatBypassActive,
            goldenDrops = systemData.goldenDrops,
            goldenRainTimer = systemData.goldenRainTimer,
            goldRushTimer = systemData.goldRushTimer,
            luckyBonus = systemData.luckyBonus,
            highestComboMultiplier = systemData.highestComboMultiplier,
            highestCritMultiplier = systemData.highestCritMultiplier,
            autoCollectorActive = systemData.isAutoCollectorActive,
            LuckyCollectorActive = systemData.isLuckyCollectorActive,
            totalAwayEarnings = systemData.totalAwayEarnings,
            unlockedAchievementIDs = new List<string>(systemData.unlockedAchievementIDs),
            currentLevel = systemData.currentLevel,
            currentXP = systemData.currentXP,
            xpToNextLevel = systemData.xpToNextLevel,
            rebirthPoints = systemData.rebirthPoints,
            critChance = systemData.critChance,
            critMultiplier = systemData.critMultiplier,
            loginStreak = systemData.loginStreak,
            lastBonusDate = systemData.lastBonusDate,
            currentDailyMultiplier = systemData.currentDailyMultiplier,
            basePPS = systemData.basePPS,
            workersPPS = systemData.workersPPS,
            clickMasteryLvl = systemData.clickMasteryLvl,
            critMasteryLvl = systemData.critMasteryLvl,
            clickMasteryXP = systemData.clickMasteryXP,
            critMasteryXP = systemData.critMasteryXP,
            comboMasteryLvl = systemData.comboMasteryLvl,
            comboMasteryXP = systemData.comboMasteryXP,
            awayMasteryLvl = systemData.awayMasteryLvl,
            awayMasteryXP = systemData.awayMasteryXP,
            wheelMultiplier = systemData.wheelMultiplier,
            wheelBonusTimer = systemData.wheelBonusTimer,
            workerSaleTimer = systemData.workerSaleTimer,
            isWorkerSaleActive = systemData.isWorkerSaleActive,
            currentWheelRewardIcon = systemData.currentWheelRewardIcon,
            riskMultiplier = systemData.riskMultiplier,
            riskBonusTimer = systemData.riskBonusTimer,
            lastRiskResultText = systemData.lastRiskResultText,
            rebirthCount = systemData.rebirthCount,
            rebirthMultiplier = systemData.rebirthMultiplier,
            currentBackground = systemData.currentBackground,
            adMultiplier = systemData.adMultiplier
        };

        string json = JsonUtility.ToJson(dataToSave, true);
        string encryptedJson = EncryptDecrypt(json);

        File.WriteAllText(savePath, encryptedJson);
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            if (System_Achievements.Instance != null) System_Achievements.Instance.EnableChecking();
            return;
        }

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

            systemData.goldenDrops = loadedData.goldenDrops;
            systemData.luckyBonus = loadedData.luckyBonus;
            systemData.goldenRainTimer = loadedData.goldenRainTimer;
            systemData.goldRushTimer = loadedData.goldRushTimer;
            systemData.highestComboMultiplier = loadedData.highestComboMultiplier;
            systemData.highestCritMultiplier = loadedData.highestCritMultiplier;

            systemData.isAntiCheatBypassActive = loadedData.antiCheatBypassActive;
            systemData.isAutoClickerActive = loadedData.autoClickActive;
            systemData.isAutoCollectorActive = loadedData.autoCollectorActive;
            systemData.isLuckyCollectorActive = loadedData.LuckyCollectorActive;

            systemData.workerLevels = loadedData.workerLevels ?? new List<int>();
            systemData.unlockedSkinIDs = loadedData.unlockedSkinIDs ?? new List<int> { 0 };
            systemData.seenSkinIDs = loadedData.seenSkinIDs ?? new List<int> { 0 };
            systemData.unlockedAchievementIDs = loadedData.unlockedAchievementIDs ?? new List<string>();

            systemData.currentLevel = loadedData.currentLevel;
            systemData.currentXP = loadedData.currentXP;
            systemData.rebirthPoints = loadedData.rebirthPoints;

            systemData.critChance = loadedData.critChance;
            systemData.critMultiplier = loadedData.critMultiplier;

            systemData.loginStreak = loadedData.loginStreak;
            systemData.lastBonusDate = loadedData.lastBonusDate;
            systemData.currentDailyMultiplier = loadedData.currentDailyMultiplier;

            systemData.basePPS = loadedData.basePPS;
            systemData.workersPPS = loadedData.workersPPS;

            systemData.clickMasteryLvl = loadedData.clickMasteryLvl;
            systemData.critMasteryLvl = loadedData.critMasteryLvl;
            systemData.clickMasteryXP = loadedData.clickMasteryXP;
            systemData.critMasteryXP = loadedData.critMasteryXP;
            systemData.comboMasteryLvl = loadedData.comboMasteryLvl;
            systemData.comboMasteryXP = loadedData.comboMasteryXP;
            systemData.awayMasteryLvl = loadedData.awayMasteryLvl;
            systemData.awayMasteryXP = loadedData.awayMasteryXP;

            systemData.wheelMultiplier = loadedData.wheelMultiplier;
            systemData.wheelBonusTimer = loadedData.wheelBonusTimer;

            systemData.workerSaleTimer = loadedData.workerSaleTimer;
            systemData.isWorkerSaleActive = loadedData.isWorkerSaleActive;
            systemData.currentWheelRewardIcon = loadedData.currentWheelRewardIcon;

            systemData.riskMultiplier = loadedData.riskMultiplier;
            systemData.riskBonusTimer = loadedData.riskBonusTimer;
            systemData.lastRiskResultText = loadedData.lastRiskResultText;

            systemData.rebirthCount = loadedData.rebirthCount;
            systemData.rebirthMultiplier = loadedData.rebirthMultiplier;
            systemData.currentBackground = loadedData.currentBackground;

            systemData.adMultiplier = loadedData.adMultiplier;

            if (loadedData.xpToNextLevel > 0)
                systemData.xpToNextLevel = loadedData.xpToNextLevel;
            else
                systemData.xpToNextLevel = 500;

            if (Timer.Instance != null) Timer.Instance.LoadSavedTime(loadedData.time);
            if (clickerSkills != null) clickerSkills.RefreshSkillsVisuals();
            if (System_Wardrobe.Instance != null) System_Wardrobe.Instance.LoadSkin(systemData.currentSkinIndex);
            if (System_Notification.Instance != null) System_Notification.Instance.CheckGlobalNotification();
            if (System_Achievements.Instance != null) System_Achievements.Instance.EnableChecking();
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load save data: {e.Message}");

            if (System_Achievements.Instance != null) System_Achievements.Instance.EnableChecking();
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
            var away = GameObject.FindAnyObjectByType<System_AwayIncome>();
            if (away != null) away.CalculateAwayIncome();
        }
    }
}