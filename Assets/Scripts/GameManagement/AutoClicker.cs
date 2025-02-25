using UnityEngine;
using UnityEngine.UI;

public class AutoClicker : MonoBehaviour
{
    [SerializeField] Button autoClickButton;
    [SerializeField] Button clickButton;
    [SerializeField] float clickInterval = 0.5f;

    [Header("Active Button")]
    [SerializeField] Image autoClickButtonImage;
    [SerializeField] Sprite neutralIcon;
    [SerializeField] Sprite activeIcon;

    bool isAutoClicking = false;

    void Start()
    {
        autoClickButton.onClick.AddListener(ToggleAutoClicker);
        autoClickButtonImage.sprite = neutralIcon;
    }

    void ToggleAutoClicker()
    {
        isAutoClicking = !isAutoClicking;
        if (isAutoClicking)
        {
            autoClickButtonImage.sprite = activeIcon;
            InvokeRepeating(nameof(AutoClick), 0f, clickInterval);
        }
        else
        {
            autoClickButtonImage.sprite = neutralIcon;
            CancelInvoke(nameof(AutoClick));
        }
    }

    void AutoClick()
    {
        clickButton.onClick.Invoke();
    }
}
