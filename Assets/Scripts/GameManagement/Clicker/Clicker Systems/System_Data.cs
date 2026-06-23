using UnityEngine;
using System.Collections.Generic;

public class System_Data : MonoBehaviour
{
    [Header("Economy:")]
    public double pointsCounterFloat = 0;
    public int pointsPerClick = 1;
    public float pointsPerSecond = 0;
    public double totalAwayEarnings = 0;
    public double basePPS = 0;
    public double workersPPS = 0;

    [Header("Workers Progress:")]
    public List<int> workerLevels = new List<int>();

    [Header("Time:")]
    public float timer = 0f;

    [Header("Skills Data:")]
    public int clickMultiplier = 1;
    public int currentSkinIndex = 0;
    public bool isAutoClickerActive = false;
    public bool isAutoCollectorActive = false;
    public bool isAntiCheatBypassActive = false;
    public bool isLuckyCollectorActive = false;
    public bool isGoldRushActive = false;
    public List<int> unlockedSkinIDs = new List<int> { 0 };
    public List<int> seenSkinIDs = new List<int> { 0 };

    [Header("Events:")]
    public int goldenDrops = 0;
    public int luckyBonus = 0;
    public float goldenRainTimer;
    public float goldRushTimer;
    public float workerSaleTimer;
    public double highestComboMultiplier = 1.0;
    public double highestCritMultiplier = 1.0;
    public bool isWorkerSaleActive;

    [Header("Achievements:")]
    public List<string> unlockedAchievementIDs = new List<string>();

    [Header("Leveling")]
    public int currentLevel = 1;
    public double currentXP = 0;
    public double xpToNextLevel = 0;
    public int rebirthPoints = 0;

    [Header("Critical Click:")]
    public float critChance = 0.10f;
    public float critMultiplier = 5.0f;

    [Header("Daily Bonus:")]
    public int loginStreak = 0;
    public string lastBonusDate = "";
    public string lastNotificationDate;
    public float currentDailyMultiplier = 1.0f;

    [Header("Mastery Levels:")]
    public int clickMasteryLvl = 0;
    public int critMasteryLvl = 0;
    public int comboMasteryLvl = 0;
    public int awayMasteryLvl = 0;
    public float clickMasteryXP = 0;
    public float critMasteryXP = 0;
    public float comboMasteryXP = 0;
    public float awayMasteryXP = 0;

    [Header("Wheel Fortune Stats:")]
    public float wheelMultiplier = 1.0f;
    public float wheelBonusTimer = 0f;
    public Sprite currentWheelRewardIcon;

    [Header("Risk & Reward Stats:")]
    public float riskMultiplier = 1.0f;
    public float riskBonusTimer = 0f;
    public string lastRiskResultText = "";

    [Header("Rebirth Stats:")]
    public int rebirthCount = 0;
    public float rebirthMultiplier = 1.0f;

    [Header("Ads:")]
    public float adMultiplier = 1.0f;

    public int currentBackground = 0;
}
