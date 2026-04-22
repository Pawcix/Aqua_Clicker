using UnityEngine;

public class System_Add : MonoBehaviour
{
    [SerializeField] System_Data data;

    public void AddPoints()
    {
        data.pointsCounter += data.pointsPerClick;
    }

    public int GetTotal()
    {
        return data.pointsCounter;
    }
}
