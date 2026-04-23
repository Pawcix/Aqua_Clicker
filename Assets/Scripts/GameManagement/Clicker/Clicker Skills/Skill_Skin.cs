using UnityEngine;
using UnityEngine.UI;

public class Skill_Skin : MonoBehaviour
{
    [SerializeField] Clicker_Skills masterSkills;
    [SerializeField] Image clickerImage;
    [SerializeField] Button changeSkinButton;
    [SerializeField] Sprite[] skins;

    void Start()
    {
        if (changeSkinButton != null)
            changeSkinButton.onClick.AddListener(ChangeSkin);
    }

    public void ChangeSkin()
    {
        if (skins.Length == 0 || masterSkills == null) return;

        AudioManager.Instance.PlaySFX("Open");
        masterSkills.currentSkinIndex = (masterSkills.currentSkinIndex + 1) % skins.Length;
        UpdateVisuals(masterSkills.currentSkinIndex);
    }

    public void LoadSkin(int index) => UpdateVisuals(index);

    void UpdateVisuals(int index)
    {
        if (clickerImage != null && skins.Length > index)
            clickerImage.sprite = skins[index];
    }
}
