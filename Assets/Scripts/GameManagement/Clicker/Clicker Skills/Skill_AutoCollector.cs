using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skill_AutoCollector : MonoBehaviour
{
    [SerializeField] Clicker_Skills masterSkills;
    [SerializeField] Button collectorButton;
    [SerializeField] Image collectorButtonImage;
    [SerializeField] Sprite neutralIcon;
    [SerializeField] Sprite activeIcon;

    [SerializeField] float collectDelay = 2f; // Czas po jakim zbiera

    void Start()
    {
        if (collectorButton != null)
            collectorButton.onClick.AddListener(ToggleAutoCollector);
    }

    void Update()
    {
        // Jeśli skill nie jest aktywny, nic nie rób
        if (masterSkills == null || !masterSkills.isAutoCollectorActive) return;

        // Znajdź wszystkie aktywne krople na scenie
        GoldenDrop_Item[] activeDrops = Object.FindObjectsByType<GoldenDrop_Item>(FindObjectsSortMode.None);

        foreach (var drop in activeDrops)
        {
            // Sprawdzamy czas życia kropli
            if (drop.ExistenceTime >= collectDelay)
            {
                drop.OnGoldenDropClicked();
            }
        }
    }

    void ToggleAutoCollector()
    {
        if (masterSkills == null) return;
        SetAutoCollectorState(!masterSkills.isAutoCollectorActive);
    }

    public void SetAutoCollectorState(bool active)
    {
        if (masterSkills == null) return;

        masterSkills.isAutoCollectorActive = active;
        if (collectorButtonImage != null)
            collectorButtonImage.sprite = active ? activeIcon : neutralIcon;
    }
}
