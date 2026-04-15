using UnityEngine;

public class Modal_Language : MonoBehaviour
{
    public GameObject languageModal;

    void Awake()
    {
        if (languageModal != null)
        {
            languageModal.SetActive(false);
        }
    }

    public void ToggleExit()
    {
        if (languageModal != null)
        {
            bool newState = !languageModal.activeSelf;
            languageModal.SetActive(newState);
        }
    }
}
