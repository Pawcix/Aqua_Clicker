using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Prefab_WorkerStats : MonoBehaviour
{
    [SerializeField] Image workerIcon;
    [SerializeField] TextMeshProUGUI levelText;

    public void Setup(Sprite icon, int level)
    {
        if (workerIcon != null) workerIcon.sprite = icon;
        if (levelText != null) levelText.text = level.ToString() + " lvl";
    }
}
