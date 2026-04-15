using UnityEngine;

public class Modal_RestartGame : MonoBehaviour
{
    public GameObject restartGameModal;

    void Awake()
    {
        if (restartGameModal != null)
        {
            restartGameModal.SetActive(false);
        }
    }

    public void ToggleSettings()
    {
        if (restartGameModal != null)
        {
            bool newState = !restartGameModal.activeSelf;
            restartGameModal.SetActive(newState);
        }
    }
}
