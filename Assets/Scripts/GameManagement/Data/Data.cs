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

    public List<int> workerLevels = new List<int>();
    public List<int> unlockedSkinIDs = new List<int>();
    public List<int> seenSkinIDs = new List<int>();
    public List<string> unlockedAchievementIDs = new List<string>();
}
