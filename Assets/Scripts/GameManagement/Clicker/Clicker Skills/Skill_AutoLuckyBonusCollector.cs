using UnityEngine;
using UnityEngine.UI;

public class Skill_AutoLuckyBonusCollector : MonoBehaviour
{
    [SerializeField] Clicker_Skills masterSkills;
    [SerializeField] Button collectorButton;
    [SerializeField] Image collectorButtonImage;
    [SerializeField] Sprite neutralIcon;
    [SerializeField] Sprite activeIcon;

    [SerializeField] float collectDelay = 1.5f;

    float autoCollectTimer = 0f;

    void Start()
    {
        if (collectorButton != null)
            collectorButton.onClick.AddListener(ToggleLuckyCollector);
    }

    void Update()
    {
        if (masterSkills == null || !masterSkills.isLuckyCollectorActive) return;

        LuckyBonus activeBonus = Object.FindFirstObjectByType<LuckyBonus>();

        if (activeBonus != null && activeBonus.IsBonusVisible())
        {
            autoCollectTimer += Time.deltaTime;

            if (autoCollectTimer >= collectDelay)
            {
                activeBonus.OnBonusClicked();
                autoCollectTimer = 0f;
            }
        }
        else
        {
            autoCollectTimer = 0f;
        }
    }

    void ToggleLuckyCollector()
    {
        if (masterSkills == null) return;
        SetLuckyCollectorState(!masterSkills.isLuckyCollectorActive);
    }

    public void SetLuckyCollectorState(bool active)
    {
        if (masterSkills == null) return;

        masterSkills.isLuckyCollectorActive = active;
        if (collectorButtonImage != null)
            collectorButtonImage.sprite = active ? activeIcon : neutralIcon;
    }
}
