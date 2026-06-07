using UnityEngine;
using UnityEngine.UI;

public class Skin_Item : MonoBehaviour
{
    [Header("Skin Config:")]
    public int skinID;

    [Header("Visuals:")]
    [Tooltip("Główny obrazek skina w szafie")]
    [SerializeField] Image skinImage;

    [SerializeField] System_NotificationSkin skinBadge;

    void Awake()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
            btn.onClick.AddListener(OnItemClicked);
    }

    void Start()
    {
        Invoke(nameof(RefreshVisuals), 0.1f);
    }

    void OnEnable()
    {
        RefreshVisuals();
    }

    public void OnItemClicked()
    {
        if (System_Wardrobe.Instance == null) return;

        System_Wardrobe.Instance.MarkSkinAsSeen(skinID);
        System_Wardrobe.Instance.SelectSkin(skinID);
    }

    public void RefreshVisuals()
    {
        if (System_Wardrobe.Instance == null || System_Wardrobe.Instance.data == null) return;

        bool isUnlocked = System_Wardrobe.Instance.IsSkinUnlocked(skinID);
        bool isNew = isUnlocked && !System_Wardrobe.Instance.data.seenSkinIDs.Contains(skinID) && skinID != 0;
        bool isEquipped = System_Wardrobe.Instance.GetCurrentSelectedID() == skinID;

        var skinData = System_Wardrobe.Instance.GetAllSkins().Find(s => s.skinID == skinID);

        if (skinData != null && skinImage != null)
        {
            if (!isUnlocked)
            {
                skinImage.sprite = skinData.lockedSprite;
                skinImage.color = new Color(0.2f, 0.2f, 0.2f, 1f);
            }
            else
            {
                skinImage.sprite = isEquipped ? skinData.highlightedSprite : skinData.skinSprite;
                skinImage.color = Color.white;
            }
        }

        if (skinBadge != null)
        {
            skinBadge.SetBadge(isNew);
        }
    }
}