using TMPro;
using UnityEngine;

public class Prefab_Skins : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI skinsText;

    public void UpdateSkinsPrefab(int unlocked, int total)
    {
        if (skinsText == null) return;
        skinsText.text = $"Skins \n{unlocked} / {total}";
    }
}
