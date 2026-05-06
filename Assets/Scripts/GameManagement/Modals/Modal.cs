using UnityEngine;

public class Modal : MonoBehaviour
{
    public GameObject modalPanel;

    public void ToggleModalSettings()
    {
        if (modalPanel != null)
        {
            bool newState = !modalPanel.activeSelf;
            modalPanel.SetActive(newState);
            if (newState) AudioManager.Instance.PlaySFX("Open");
        }
    }

    public void OpenWithOrder(int orderValue)
    {
        if (modalPanel != null)
        {
            modalPanel.SetActive(true);
            Canvas canvas = modalPanel.GetComponent<Canvas>();

            if (canvas != null)
            {
                canvas.overrideSorting = true;
                canvas.sortingOrder = orderValue;
            }

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX("Open");
        }
    }

    public void CloseModal(GameObject targetModal)
    {
        if (targetModal != null)
        {
            targetModal.SetActive(false);
        }
    }
}