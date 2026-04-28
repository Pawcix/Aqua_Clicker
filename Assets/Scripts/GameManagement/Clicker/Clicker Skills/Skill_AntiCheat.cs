using UnityEngine;
using UnityEngine.UI;

public class Skill_AntiCheat : MonoBehaviour
{
    [SerializeField] Clicker_Skills masterSkills;
    [SerializeField] Button bypassButton;
    [SerializeField] Image bypassButtonImage;
    [SerializeField] Sprite neutralIcon;
    [SerializeField] Sprite activeIcon;

    void Start()
    {
        if (bypassButton != null)
            bypassButton.onClick.AddListener(ToggleBypass);
    }

    void ToggleBypass()
    {
        if (masterSkills == null) return;
        SetBypassState(!masterSkills.isAntiCheatBypassActive);
    }

    public void SetBypassState(bool active)
    {
        if (masterSkills == null) return;

        masterSkills.isAntiCheatBypassActive = active;
        if (bypassButtonImage != null)
            bypassButtonImage.sprite = active ? activeIcon : neutralIcon;

        // Debug.Log($"<color=cyan>[Skill]</color> AntiCheat Bypass: {active}");
    }
}
