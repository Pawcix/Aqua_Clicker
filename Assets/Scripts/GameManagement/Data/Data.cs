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
    public float goldenRainTimer;
    public float goldRushTimer;
    public float time;
    public bool autoClickActive;
    public bool autoCollectorActive;
    public bool antiCheatBypassActive;
    public bool LuckyCollectorActive;
    public List<int> workerLevels;
    public List<int> unlockedSkinIDs;
    // public List<HistoryList> awayHistory = new List<HistoryList>();
}
