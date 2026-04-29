using UnityEngine;

public class System_Add : MonoBehaviour
{
    [SerializeField] System_Data data;

    public void AddPoints()
    {
        int pointsToAdd = data.pointsPerClick * data.clickMultiplier;
        data.pointsCounter += pointsToAdd;
    }

    public int GetTotal()
    {
        return data.pointsCounter;
    }
}
