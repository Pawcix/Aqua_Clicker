using UnityEngine;

[CreateAssetMenu(fileName = "NewWorker", menuName = "Clicker/Worker")]
public class Worker : ScriptableObject
{
    [Header("Main Information")]
    public string workerName;
    public Sprite icon;

    [Header("Stats:")]
    public string description;
    public float basePower;
    public double basePrice;
    public float priceMultiplier = 1.15f;

    public Worker Clone()
    {
        return (Worker)this.MemberwiseClone();
    }

    public double GetPriceForLevel(int level)
    {
        return basePrice * System.Math.Pow((double)priceMultiplier, level);
    }
}