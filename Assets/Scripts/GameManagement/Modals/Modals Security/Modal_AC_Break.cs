using TMPro;
using UnityEngine;

public class Modal_AC_Break : MonoBehaviour
{
    public static Modal_AC_Break Instance;

    [Header("UI Elements:")]
    [SerializeField] GameObject breakModal;
    [SerializeField] TextMeshProUGUI sessionTimeText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        if (breakModal != null)
        {
            breakModal.SetActive(false);
        }
    }

    public void ShowModal(string timeSpent)
    {
        if (breakModal != null)
        {
            breakModal.SetActive(true);

            if (sessionTimeText != null)
            {
                sessionTimeText.text = $"Session: <color=yellow>{timeSpent}</color>";
            }
        }
    }

    public void OnCloseClicked()
    {
        if (breakModal != null)
        {
            breakModal.SetActive(false);
        }
    }
}
