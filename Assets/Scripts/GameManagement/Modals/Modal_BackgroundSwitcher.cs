using UnityEngine;

public class Modal_BackgroundSwitcher : MonoBehaviour
{
    public GameObject backgroundSwitcherModal;

    void Awake()
    {
        if (backgroundSwitcherModal != null)
        {
            backgroundSwitcherModal.SetActive(false);
        }
    }

    public void ToggleExit()
    {
        if (backgroundSwitcherModal != null)
        {
            bool newState = !backgroundSwitcherModal.activeSelf;
            backgroundSwitcherModal.SetActive(newState);
        }
    }
}
