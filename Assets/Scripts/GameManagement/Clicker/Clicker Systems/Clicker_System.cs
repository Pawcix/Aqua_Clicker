using UnityEngine;
using UnityEngine.Events;

public class Clicker_System : MonoBehaviour
{
    public static UnityEvent<int, int> OnItemBought = new UnityEvent<int, int>();

    [Header("Data Source:")]
    [SerializeField] System_Data data;

    [Header("Stats & Prefabs:")]
    [SerializeField] Clicker_Stats clickerStats;
    [SerializeField] Clicker_Prefabs clickerPrefabs;
    [SerializeField] Clicker_Skills clickerSkills;

    [Header("Systems:")]
    [SerializeField] System_Add addSystem;
    [SerializeField] System_WordsEffect clickWords;

    public void Click()
    {
        if (addSystem == null || data == null) return;

        addSystem.AddPoints();
        int currentTotal = addSystem.GetTotal();
        int currentPPS = data.pointsPerSecond;

        if (clickerPrefabs != null)
        {
            clickerPrefabs.UpdateAllPrefabs(currentTotal, currentPPS);
        }

        if (clickerStats != null)
        {
            clickerStats.UpdateAllStats(currentTotal, currentPPS);
        }

        if (clickerSkills != null)
        {
            clickerSkills.UpdateAllSkills(currentTotal);
        }

        if (clickWords != null)
        {
            clickWords.ShowRandomWord();
        }
    }
}
