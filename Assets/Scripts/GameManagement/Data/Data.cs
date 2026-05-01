using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public float score;
    public int pps;
    public int skinIndex;
    public int totalAwayEarnings;
    public int clickMultiplier;
    public int goldenDrops;
    public int luckyBonus;
    public float goldenRainTimer;
    public float time;
    public bool autoClickActive;
    public bool autoCollectorActive;
    public bool antiCheatBypassActive;
    public List<int> workerLevels;
    public List<int> unlockedSkinIDs;
}
