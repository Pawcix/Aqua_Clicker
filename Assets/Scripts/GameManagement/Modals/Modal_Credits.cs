using UnityEngine;

public class Modal_Credits : MonoBehaviour
{
    public GameObject creditsModal;

    void Awake()
    {
        if (creditsModal != null)
        {
            creditsModal.SetActive(false);
        }
    }

    public void ToggleSettings()
    {
        if (creditsModal != null)
        {
            bool newState = !creditsModal.activeSelf;
            creditsModal.SetActive(newState);
        }
    }
}
