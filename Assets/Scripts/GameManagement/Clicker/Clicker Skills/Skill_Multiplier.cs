using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Multiplier : MonoBehaviour
{
    [SerializeField] Clicker_Skills masterSkills;
    [SerializeField] public Button multiplierButton;
    [SerializeField] public int multiplierValue = 1;
    [SerializeField] public int requiredLevel = 0;

    [Header("Visuals:")]
    [SerializeField] Image buttonImage;
    [SerializeField] Sprite activeSprite;
    [SerializeField] Sprite neutralSprite;
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
        if (masterSkills.data.clickMultiplier == multiplierValue)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX("Skill - Off");
            }
            return;
        }

        masterSkills.data.clickMultiplier = multiplierValue;
        masterSkills.RefreshMultiplierButtons();

        if (AudioManager.Instance != null)
        {
            if (multiplierValue == 1)
            {
                AudioManager.Instance.PlaySFX("Skill - Off");
            }
            else
            {
                AudioManager.Instance.PlaySFX("Skill - On");
            }
        }
    }

    void OnEnable()
    {
        RefreshVisuals();
    }
}