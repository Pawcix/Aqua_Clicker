using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Multiplier : MonoBehaviour
{
    [SerializeField] Clicker_Skills masterSkills;
    [SerializeField] public Button multiplierButton;
    [SerializeField] public int multiplierValue = 2;

    [Header("Visuals:")]
    [SerializeField] Image buttonImage;
    [SerializeField] Sprite neutralSprite;
    [SerializeField] Sprite activeSprite;
    [SerializeField] public TextMeshProUGUI reqText;

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
