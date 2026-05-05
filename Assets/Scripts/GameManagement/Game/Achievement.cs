using UnityEngine;

public enum AchievementType
{
    TotalPoints,
    GoldenDrops,
    TotalClicks
}

[CreateAssetMenu(fileName = "New Achievement", menuName = "Achievements/Achievement")]
public class Achievement : ScriptableObject
{
    public string id;
    public string title;
    public Sprite icon;

    [Header("Requirements")]
    public AchievementType type;
    public double requiredValue;

    [Header("Reward")]
    public int rewardSkinID = -1;
}