using UnityEngine;

public class System_Economy : MonoBehaviour
{
    public static System_Economy Instance;

    [SerializeField] System_Data data;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public long TotalPointsInt
    {
        get => (long)System.Math.Floor(data.pointsCounterFloat);
        set => data.pointsCounterFloat = value;
    }

    public double TotalPointsDouble
    {
        get => data.pointsCounterFloat;
        set => data.pointsCounterFloat = value;
    }

    public void AddPoints(double amount)
    {
        data.pointsCounterFloat += amount;
    }

    public void AddPoints(float amount)
    {
        data.pointsCounterFloat += (double)amount;
    }
}
