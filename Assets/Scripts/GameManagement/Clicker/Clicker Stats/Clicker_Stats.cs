using UnityEngine;

public class Clicker_Stats : MonoBehaviour
{
    [SerializeField] Total_Points totalPointsDisplay;
    [SerializeField] Total_PPS totalPPSDisplay;

    public void UpdateAllStats(double totalPoints, double totalPPS)
    {
        if (totalPointsDisplay != null)
        {
            totalPointsDisplay.UpdateDisplay(totalPoints);
        }

        if (totalPPSDisplay != null)
        {
            totalPPSDisplay.UpdatePPS(totalPPS);
        }
    }
}
