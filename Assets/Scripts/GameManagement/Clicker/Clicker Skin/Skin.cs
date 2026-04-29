using UnityEngine;

[System.Serializable]
public class ClickerSkin
{
    public int skinID;
    public string skinName;
    public bool isFreeAtStart;
    public Sprite skinSprite;
    public Sprite lockedSprite;
    public SkinRarity rarity;
}

public enum SkinRarity { Common, Rare, Legendary, Secret }