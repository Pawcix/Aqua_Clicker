using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Clicker/New Worker")]

public class Worker : ScriptableObject
{
    [Header("General")]
    public new string name;
    public int power;

    [Header("Cost")]
    public int pricePower;

    public Worker Clone()
    {
        return (Worker)this.MemberwiseClone();
    }
}
