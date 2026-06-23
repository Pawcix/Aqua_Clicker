using UnityEngine;

public class System_Economy : MonoBehaviour
{
    public static System_Economy Instance;

    [SerializeField] System_Data data;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public double GetPoints()
    {
        return data.pointsCounterFloat;
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

    public void AddPoints(double amount, bool giveXP = true)
    {
        data.pointsCounterFloat += amount;

        if (giveXP && System_Leveling.Instance != null)
        {
            System_Leveling.Instance.RegisterPointGain(amount);
        }
    }

    public void AddPoints(float amount)
    {
        AddPoints((double)amount, true);
    }
}