using UnityEngine;
using UnityEngine.UI;

public class AutoClicker : MonoBehaviour
{
    [SerializeField] Button autoClickButton;
    [SerializeField] Button clickButton;
    [SerializeField] float clickInterval = 0.5f;

    bool isAutoClicking = false;

    void Start()
    {
        autoClickButton.onClick.AddListener(ToggleAutoClicker);
    }

    void ToggleAutoClicker()
    {
        isAutoClicking = !isAutoClicking;
        if (isAutoClicking)
        {
            InvokeRepeating("AutoClick", 0f, clickInterval);
        }
        else
        {
            CancelInvoke("AutoClick");
        }
    }

    void AutoClick()
    {
        clickButton.onClick.Invoke();
    }
}
