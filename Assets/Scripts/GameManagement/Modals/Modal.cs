using UnityEngine;

public class Modal : MonoBehaviour
{
    public GameObject modalPanel;

    public void ToggleModalSettings()
    {
        if (modalPanel != null)
        {
            if (!modalPanel.activeInHierarchy)
            {
                modalPanel.SetActive(true);
                if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Modal - Open and Close");
            }
            else
            {
                CloseModal(modalPanel);
            }
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
                AudioManager.Instance.PlaySFX("Modal - Open and Close");
        }
    }

    public void CloseModal(GameObject targetModal)
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Modal - Open and Close");
        if (targetModal == null) return;
        if (targetModal.activeInHierarchy)
        {
            UI_JuicyModal juicyAnim = targetModal.GetComponent<UI_JuicyModal>();

            if (juicyAnim != null)
            {
                juicyAnim.CloseModal();
                return;
            }
        }

        targetModal.SetActive(false);

    }
}