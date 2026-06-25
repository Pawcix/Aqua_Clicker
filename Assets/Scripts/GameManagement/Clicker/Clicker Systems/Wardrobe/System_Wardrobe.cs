using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class System_Wardrobe : MonoBehaviour
{
    public static System_Wardrobe Instance;

    [Header("References:")]
    [SerializeField] System_WardrobeProgressBar wardrobeProgressBar;
    [SerializeField] Clicker_Skills masterSkills;

    [Tooltip("Główny komponent Image na przycisku kropli")]
    [SerializeField] Image mainClickerDisplay;

    [Tooltip("Główny komponent Button na przycisku kropli (aby zmieniać Sprite Swap)")]
    [SerializeField] Button mainClickerButton;

    [Header("Scroll Settings:")]
    [SerializeField] Scrollbar wardrobeScrollbar;
    [SerializeField] ScrollRect wardrobeScrollRect;

    [Header("Skin List:")]
    public List<ClickerSkin> allSkins;

    void Start()
    {
        if (masterSkills != null && masterSkills.data != null)
        {
            foreach (var skin in allSkins)
            {
                if (skin.isFreeAtStart && !masterSkills.data.unlockedSkinIDs.Contains(skin.skinID))
                {
                    masterSkills.data.unlockedSkinIDs.Add(skin.skinID);
                }
            }
        }

        RefreshProgressBar();
    }

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void OnEnable()
    {
        if (wardrobeScrollbar != null)
        {
            StartCoroutine(ResetScrollRoutine());
        }

        RefreshProgressBar();
    }

    public int GetCurrentSelectedID()
    {
        if (masterSkills == null) return -1;
        return masterSkills.currentSkinIndex;
    }

    public List<ClickerSkin> GetAllSkins() => allSkins;

    public void RefreshAllItemFrames()
    {
        Skin_Item[] allItems = Object.FindObjectsByType<Skin_Item>(FindObjectsInactive.Exclude);

        foreach (var item in allItems)
        {
            item.RefreshVisuals();
        }
    }

    public bool IsSkinUnlocked(int id)
    {
        if (masterSkills == null || masterSkills.data == null) return false;
        return masterSkills.data.unlockedSkinIDs.Contains(id);
    }

    public bool IsSkinNew(int id)
    {
        if (masterSkills == null || masterSkills.data == null) return false;

        bool unlocked = IsSkinUnlocked(id);
        bool seen = masterSkills.data.seenSkinIDs.Contains(id);

        return unlocked && !seen;
    }

    public void MarkSkinAsSeen(int id)
    {
        if (masterSkills == null || masterSkills.data == null) return;

        if (!masterSkills.data.seenSkinIDs.Contains(id))
        {
            masterSkills.data.seenSkinIDs.Add(id);

            if (Data_SaveManager.instance != null) Data_SaveManager.instance.SaveGame();
            if (System_Notification.Instance != null)
                System_Notification.Instance.CheckGlobalNotification();

            RefreshAllItemFrames();
        }
    }

    public void SelectSkin(int id)
    {
        if (!IsSkinUnlocked(id))
        {
            AudioManager.Instance.PlaySFX("Locked");
            return;
        }

        var skin = GetAllSkins().Find(s => s.skinID == id);
        if (skin != null)
        {
            masterSkills.currentSkinIndex = id;

            UpdateMainDisplay(skin);

            RefreshProgressBar();

            AudioManager.Instance.PlaySFX("Equip");
            RefreshAllItemFrames();
        }
    }

    public void LoadSkin(int id)
    {
        var skin = GetAllSkins().Find(s => s.skinID == id);
        if (skin != null) UpdateMainDisplay(skin);
    }

    void UpdateMainDisplay(ClickerSkin skin)
    {
        if (mainClickerDisplay != null)
        {
            mainClickerDisplay.sprite = skin.skinSprite;
        }

        if (mainClickerButton != null)
        {
            SpriteState state = mainClickerButton.spriteState;

            state.highlightedSprite = skin.highlightedSprite;
            state.pressedSprite = skin.pressedSprite;

            mainClickerButton.spriteState = state;
        }
    }

    public int GetUnlockedSkinsCount()
    {
        if (masterSkills != null && masterSkills.data != null && masterSkills.data.unlockedSkinIDs != null)
        {
            return masterSkills.data.unlockedSkinIDs.Count;
        }
        return 0;
    }

    public void RefreshProgressBar()
    {
        if (wardrobeProgressBar != null)
        {
            wardrobeProgressBar.UpdateProgressBar();
        }
    }

    public void UnlockSkin(int skinID)
    {
        if (masterSkills == null || masterSkills.data == null) return;

        if (!masterSkills.data.unlockedSkinIDs.Contains(skinID))
        {
            masterSkills.data.unlockedSkinIDs.Add(skinID);

            RefreshAllItemFrames();
            RefreshProgressBar();

            if (System_Notification.Instance != null)
                System_Notification.Instance.CheckGlobalNotification();

            if (Data_SaveManager.instance != null) Data_SaveManager.instance.SaveGame();
        }
    }

    public System_Data data => masterSkills != null ? masterSkills.data : null;

    IEnumerator ResetScrollRoutine()
    {
        yield return new WaitForEndOfFrame();

        wardrobeScrollbar.value = 1f;

        if (wardrobeScrollRect != null)
        {
            wardrobeScrollRect.verticalNormalizedPosition = 1f;
        }
    }
}