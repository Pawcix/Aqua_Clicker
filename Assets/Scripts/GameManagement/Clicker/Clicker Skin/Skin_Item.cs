using UnityEngine;
using UnityEngine.UI;

public class Skin_Item : MonoBehaviour
{
    [Header("Skin Config:")]
    public int skinID;

    [Header("Visuals:")]
    [SerializeField] Image frameImage;
    [SerializeField] Sprite normalFrame;
    [SerializeField] Sprite selectedFrame;
    // [SerializeField] System_NotificationSkin skinBadge;

    Button button;
    System_Wardrobe wardrobe;

    void Awake()
    {
        wardrobe = Object.FindFirstObjectByType<System_Wardrobe>();

        Button btn = GetComponent<Button>();
        if (btn != null)
            btn.onClick.AddListener(OnItemClicked);
    }

    void Start()
    {
        if (wardrobe == null) wardrobe = Object.FindFirstObjectByType<System_Wardrobe>();

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
        if (System_Wardrobe.Instance == null || frameImage == null) return;

        int myID = skinID;
        bool isUnlocked = System_Wardrobe.Instance.IsSkinUnlocked(myID);

        var skinData = System_Wardrobe.Instance.GetAllSkins().Find(s => s.skinID == myID);

        if (skinData != null)
        {
            frameImage.sprite = isUnlocked ? skinData.skinSprite : skinData.lockedSprite;
            frameImage.color = isUnlocked ? Color.white : new Color(0.2f, 0.2f, 0.2f, 1f);
        }

        // if (skinBadge != null)
        // {
        //     bool isNew = System_Wardrobe.Instance.IsSkinNew(skinID);
        //     skinBadge.SetBadge(isNew);
        // }
    }
}
