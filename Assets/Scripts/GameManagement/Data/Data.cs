using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public double score;
    public int pps;
    public int skinIndex;
    public double totalAwayEarnings;
    public int clickMultiplier;
    public int goldenDrops;
    public int luckyBonus;
    public double highestComboMultiplier;
    public float goldenRainTimer;
    public float time;
    public bool autoClickActive;
    public bool autoCollectorActive;
    public bool antiCheatBypassActive;
    public bool LuckyCollectorActive;
    public List<int> workerLevels;
    public List<int> unlockedSkinIDs;
}
