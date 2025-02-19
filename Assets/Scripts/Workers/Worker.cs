using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Clicker/New Worker")]

public class Worker : ScriptableObject
{
    [Header("General")]
    public new string name;
    public int power;
    public int powerUpgrade;

    [Header("Cost")]
    public int pricePower;
    public int priceUpgrade;

    [Header("Primary Cost")]
    public int initialPriceUpgrade;
    public int initialPowerUpgrade;

    public Worker Clone()
    {
        return (Worker)this.MemberwiseClone();
    }
}
