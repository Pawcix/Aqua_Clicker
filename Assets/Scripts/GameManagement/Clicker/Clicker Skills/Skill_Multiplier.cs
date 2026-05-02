using UnityEngine;
using UnityEngine.UI;

public class Skill_Multiplier : MonoBehaviour
{
    [SerializeField] Clicker_Skills masterSkills;
    [SerializeField] Button multiplierButton;
    [SerializeField] int multiplierValue = 2;

    [Header("Visuals:")]
    [SerializeField] Image buttonImage;
    [SerializeField] Sprite neutralSprite;
    [SerializeField] Sprite activeSprite;

    void Start()
    {
        if (multiplierButton != null)
        {
            multiplierButton.onClick.RemoveAllListeners();
            multiplierButton.onClick.AddListener(SelectThisMultiplier);
        }

        RefreshVisuals();
    }

    public void RefreshVisuals()
    {
        if (masterSkills == null || masterSkills.data == null || buttonImage == null) return;

        bool isActive = masterSkills.data.clickMultiplier == multiplierValue;
        buttonImage.sprite = isActive ? activeSprite : neutralSprite;
    }

    void SelectThisMultiplier()
    {
        if (masterSkills == null || masterSkills.data == null) return;

        masterSkills.data.clickMultiplier = multiplierValue;
        masterSkills.RefreshMultiplierButtons();

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("Select");
    }

    void OnEnable()
    {
        RefreshVisuals();
    }
}
