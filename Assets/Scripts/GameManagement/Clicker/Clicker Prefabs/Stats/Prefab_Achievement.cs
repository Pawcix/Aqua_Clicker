using TMPro;
using UnityEngine;

public class Prefab_Achievement : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI achievementsText;

    public void UpdateAchievementsPrefab(int unlocked, int total)
    {
        if (achievementsText == null) return;
        achievementsText.text = $"Achievements \n{unlocked} / {total}";
    }
}
