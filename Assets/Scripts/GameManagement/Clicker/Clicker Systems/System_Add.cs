using UnityEngine;

public class System_Add : MonoBehaviour
{
    [SerializeField] System_Data data;

    public void AddPoints()
    {
        float pointsToAdd = (float)data.pointsPerClick * data.clickMultiplier;
        System_Economy.Instance.AddPoints(pointsToAdd);
    }

    public int GetTotal()
    {
        return Mathf.RoundToInt(System_Economy.Instance.TotalPointsFloat);
    }
}
