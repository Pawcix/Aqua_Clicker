using UnityEngine;
using UnityEngine.UI;

public class System_Tabs : MonoBehaviour
{
    [Header("Panels Content")]
    [SerializeField] GameObject[] allPanels;

    [Header("Buttons (Optional)")]
    [SerializeField] Button[] tabButtons;
    [SerializeField] Color activeColor = Color.white;
    [SerializeField] Color inactiveColor = Color.gray;

    void Start()
    {
        SwitchTab(0);
    }

    public void SwitchTab(int tabIndex)
    {
        for (int i = 0; i < allPanels.Length; i++)
        {
            allPanels[i].SetActive(i == tabIndex);
        }

        for (int i = 0; i < tabButtons.Length; i++)
        {
            if (tabButtons[i] != null)
            {
                tabButtons[i].image.color = (i == tabIndex) ? activeColor : inactiveColor;
            }
        }

        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Click");
    }
}