using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public double score;
    public float pps;
    public int skinIndex;
    public double totalAwayEarnings;
    public int clickMultiplier;
    public int goldenDrops;
    public int luckyBonus;
    public double highestComboMultiplier;
    public double highestCritMultiplier;
    public float goldenRainTimer;
    public float goldRushTimer;
    public float time;
    public bool autoClickActive;
    public bool autoCollectorActive;
    public bool antiCheatBypassActive;
    public bool LuckyCollectorActive;

    public List<int> workerLevels = new List<int>();
    public List<int> unlockedSkinIDs = new List<int>();
    public List<int> seenSkinIDs = new List<int>();
    public List<string> unlockedAchievementIDs = new List<string>();

    public int currentLevel;
    public int rebirthPoints;
    public double currentXP;
    public double xpToNextLevel;

    public float critChance;
    public float critMultiplier;

    public int loginStreak;
    public string lastBonusDate;
    public float currentDailyMultiplier;

    public double basePPS;
    public double workersPPS;

    public int clickMasteryLvl;
    public int critMasteryLvl;
    public int comboMasteryLvl;
    public int awayMasteryLvl;
    public float clickMasteryXP;
    public float critMasteryXP;
    public float comboMasteryXP;
    public float awayMasteryXP;

    public float wheelMultiplier;
    public float wheelBonusTimer;

    public float workerSaleTimer;
    public bool isWorkerSaleActive;
    public Sprite currentWheelRewardIcon;

    public float riskMultiplier;
    public float riskBonusTimer;
    public string lastRiskResultText;

    public int rebirthCount;
    public float rebirthMultiplier;
}
