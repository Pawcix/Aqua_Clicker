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

    public int TotalPointsInt
    {
        get => (int)data.pointsCounterFloat;
        set => data.pointsCounterFloat = value;
    }

    public float TotalPointsFloat
    {
        get => data.pointsCounterFloat;
        set => data.pointsCounterFloat = value;
    }

    public void AddPoints(float amount)
    {
        data.pointsCounterFloat += amount;
    }
}
