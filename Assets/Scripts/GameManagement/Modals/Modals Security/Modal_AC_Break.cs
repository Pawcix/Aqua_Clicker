using UnityEngine;

public class Modal_AC_Break : MonoBehaviour
{
    public static Modal_AC_Break Instance;

    [Header("UI Elements:")]
    [SerializeField] GameObject breakModal;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        if (breakModal != null)
        {
            breakModal.SetActive(false);
        }
    }

    public void ShowModal()
    {
        if (breakModal != null)
        {
            breakModal.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void OnCloseClicked()
    {
        if (breakModal != null)
        {
            breakModal.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
