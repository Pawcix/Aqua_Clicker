using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int score;
    public int pps;
    public int skinIndex;
    public int totalAwayEarnings;
    public float time;
    public bool autoClickActive;
    public bool antiCheatBypassActive;
    public List<int> workerLevels;
}
