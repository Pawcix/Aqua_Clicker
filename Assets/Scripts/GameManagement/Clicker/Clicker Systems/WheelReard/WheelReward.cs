using UnityEngine;

[System.Serializable]
public class WheelReward
{
    public string rewardName;
    public Sprite rewardIcon;
    public enum RewardType { Multiplier }
    public RewardType type;
    public double value;
}