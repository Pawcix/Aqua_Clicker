using UnityEngine;
using UnityEngine.UI;

public class Skins : MonoBehaviour
{
    [SerializeField] Button clickerButton;
    [SerializeField] Button changeSkinButton;
    [SerializeField] Image clickerImage;
    [SerializeField] Sprite[] skins;

    int currentSkinIndex = 0;

    void Start()
    {
        if (changeSkinButton != null)
        {
            changeSkinButton.onClick.AddListener(ChangeSkin);
        }
    }

    void ChangeSkin()
    {
        if (skins.Length == 0) return;

        currentSkinIndex = (currentSkinIndex + 1) % skins.Length;
        clickerImage.sprite = skins[currentSkinIndex];
    }
}