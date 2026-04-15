using UnityEngine;

public class Modal_Exit : MonoBehaviour
{
    public GameObject exitModal;

    void Awake()
    {
        if (exitModal != null)
        {
            exitModal.SetActive(false);
        }
    }

    public void ToggleExit()
    {
        if (exitModal != null)
        {
            bool newState = !exitModal.activeSelf;
            exitModal.SetActive(newState);
        }
    }
}
