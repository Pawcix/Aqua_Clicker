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

    public double GetTotalCost(int currentLevel, int count)
    {
        double r = priceMultiplier;
        double a = basePrice * System.Math.Pow(r, currentLevel);
        return a * (System.Math.Pow(r, count) - 1) / (r - 1);
    }

    public int GetMaxAffordable(int currentLevel, double currentPoints)
    {
        double r = priceMultiplier;
        double a = basePrice * System.Math.Pow(r, currentLevel);
        return (int)System.Math.Floor(System.Math.Log((currentPoints * (r - 1) / a) + 1, r));
    }
}