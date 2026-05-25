using UnityEngine;

public class Modal_Credits : MonoBehaviour
{
    public GameObject creditsModal;
    [SerializeField] private Credits creditsScript;

    void Awake()
    {
        if (creditsModal != null)
        {
            creditsModal.SetActive(false);
        }
    }

    public void ToggleSettings()
    {
        if (creditsModal == null) return;

        bool newState = !creditsModal.activeSelf;
        creditsModal.SetActive(newState);

        if (creditsScript != null)
        {
            if (newState)
            {
                creditsScript.ResetAndStartScroll();
            }
            else
            {
                creditsScript.StopScroll();
            }
        }
    }
}