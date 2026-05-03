using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Prefab_Achievements : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] CanvasGroup canvasGroup;

    public void Setup(Achievement ach, bool isUnlocked)
    {
        iconImage.sprite = ach.icon;
        titleText.text = ach.title;

        if (isUnlocked)
        {
            canvasGroup.alpha = 1.0f;
            iconImage.color = Color.white;
        }
        else
        {
            canvasGroup.alpha = 0.4f;
            iconImage.color = Color.gray;
        }
    }
}
