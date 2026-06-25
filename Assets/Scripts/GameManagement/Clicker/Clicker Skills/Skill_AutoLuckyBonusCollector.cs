using UnityEngine;
using UnityEngine.UI;

public class Skill_AutoLuckyBonusCollector : MonoBehaviour
{
    [SerializeField] Clicker_Skills masterSkills;
    [SerializeField] public Button collectorButton;
    [SerializeField] Image collectorButtonImage;
    [SerializeField] Sprite neutralIcon;
    [SerializeField] Sprite activeIcon;

    [SerializeField] float collectDelay = 1.5f;

    float autoCollectTimer = 0f;
    private LuckyBonus _luckyBonusRef;

    void Start()
    {
        if (collectorButton != null)
            collectorButton.onClick.AddListener(ToggleLuckyCollector);

        _luckyBonusRef = Object.FindAnyObjectByType<LuckyBonus>();
    }

    void Update()
    {
        if (masterSkills == null || !masterSkills.isLuckyCollectorActive) return;

        if (_luckyBonusRef == null) _luckyBonusRef = Object.FindAnyObjectByType<LuckyBonus>();

        if (_luckyBonusRef != null && _luckyBonusRef.IsBonusVisible())
        {
            autoCollectTimer += Time.deltaTime;

            if (autoCollectTimer >= collectDelay)
            {
                _luckyBonusRef.OnBonusClicked();
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