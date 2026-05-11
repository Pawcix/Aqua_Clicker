using UnityEngine;

[System.Serializable]
public class WheelReward
{
    public string rewardName; 
    public Sprite rewardIcon;
    public float chance;

    public enum RewardType { Points, Multiplier, MasteryXP, GoldenDrop }
    public RewardType type;
    public double value;
}