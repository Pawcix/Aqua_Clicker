using UnityEngine;

public class Clicker_System : MonoBehaviour
{
    [Header("Stats & Prefabs:")]
    [SerializeField] Clicker_Stats clickerStats;
    [SerializeField] Clicker_Prefabs clickerPrefabs;

    [Header("Systems:")]
    [SerializeField] System_Add addSystem;
    [SerializeField] System_WordsEffect clickWords;

    public void Click()
    {
        if (addSystem == null) return;

        addSystem.AddPoints();
        int currentTotal = addSystem.GetTotal();

        if (clickerPrefabs != null)
        {
            clickerPrefabs.UpdateAllPrefabs(currentTotal);
        }

        if (clickerStats != null)
        {
            clickerStats.UpdateAllStats(currentTotal);
        }

        if (clickWords != null)
        {
            clickWords.ShowRandomWord();
        }
    }
}
