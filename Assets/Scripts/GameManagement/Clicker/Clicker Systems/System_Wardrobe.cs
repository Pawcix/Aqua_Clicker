using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class System_Wardrobe : MonoBehaviour
{
    public static System_Wardrobe Instance;

    [Header("References:")]
    [SerializeField] Clicker_Skills masterSkills;
    [SerializeField] Image mainClickerDisplay;

    [Header("Scroll Settings:")]
    [SerializeField] Scrollbar wardrobeScrollbar;
    [SerializeField] ScrollRect wardrobeScrollRect;

    [Header("Skin Lists:")]
    public List<ClickerSkin> commonSkins;
    public List<ClickerSkin> rareSkins;
    public List<ClickerSkin> legendarySkins;
    public List<ClickerSkin> secretSkins;

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
    }

    public int GetCurrentSelectedID()
    {
        if (masterSkills == null) return -1;
        return masterSkills.currentSkinIndex;
    }

    public List<ClickerSkin> GetAllSkins() =>
        commonSkins.Concat(rareSkins).Concat(legendarySkins).Concat(secretSkins).ToList();

    public void RefreshAllItemFrames()
    {
        Skin_Item[] allItems = Object.FindObjectsByType<Skin_Item>(FindObjectsSortMode.None);
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

    public void SelectSkin(int id)
    {
        if (!IsSkinUnlocked(id))
        {
            Debug.Log("Skin jest zablokowany!");
            AudioManager.Instance.PlaySFX("Locked");
            return;
        }

        var skin = GetAllSkins().Find(s => s.skinID == id);
        if (skin != null)
        {
            masterSkills.currentSkinIndex = id;
            if (mainClickerDisplay != null) mainClickerDisplay.sprite = skin.skinSprite;

            AudioManager.Instance.PlaySFX("Equip");
            RefreshAllItemFrames();
        }
    }

    public void LoadSkin(int id)
    {
        var skin = GetAllSkins().Find(s => s.skinID == id);
        if (skin != null) UpdateMainDisplay(skin.skinSprite);
    }

    void UpdateMainDisplay(Sprite sp)
    {
        if (mainClickerDisplay != null) mainClickerDisplay.sprite = sp;
    }

    public int GetUnlockedSkinsCount()
    {
        // Musimy się odwołać do danych przez masterSkills, tak jak w metodzie IsSkinUnlocked
        if (masterSkills != null && masterSkills.data != null && masterSkills.data.unlockedSkinIDs != null)
        {
            return masterSkills.data.unlockedSkinIDs.Count;
        }
        return 0;
    }

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
