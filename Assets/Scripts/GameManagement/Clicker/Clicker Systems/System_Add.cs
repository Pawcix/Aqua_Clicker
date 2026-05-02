using UnityEngine;

public class System_Add : MonoBehaviour
{
    [SerializeField] System_Data data;

    public void AddPoints()
    {
        double pointsToAdd = (double)data.pointsPerClick * data.clickMultiplier;

        System_Economy.Instance.AddPoints(pointsToAdd);
    }

    public long GetTotal()
    {
        return (long)System.Math.Floor(System_Economy.Instance.TotalPointsDouble);
    }
}
