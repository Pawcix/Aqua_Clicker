using UnityEngine;
using System.Collections.Generic;

public class System_Data : MonoBehaviour
{
    [Header("Economy:")]
    public double pointsCounterFloat = 0;
    public int pointsPerClick = 1;
    public float pointsPerSecond = 0;
    public double totalAwayEarnings = 0;
    // public List<HistoryList> awayHistory = new List<HistoryList>();

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

    [Header("Events:")]
    public int goldenDrops = 0;
    public float goldenRainTimer = 300f;
    public float goldRushTimer = 300f;
    public int luckyBonus = 0;
    public double highestComboMultiplier = 1.0;
}
