using UnityEngine;

public class Clicker_Stats : MonoBehaviour
{
    [SerializeField] Total_Points totalPointsDisplay;

    public void UpdateAllStats(int totalPoints)
    {
        if (totalPointsDisplay != null)
        {
            totalPointsDisplay.UpdateDisplay(totalPoints);
        }
    }
}
