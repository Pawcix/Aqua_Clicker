using UnityEngine;

public class Modal_AC_JSON : MonoBehaviour
{
    public static Modal_AC_JSON Instance;

    [Header("UI Elements:")]
    [SerializeField] GameObject antiCheatJsonModal;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        if (antiCheatJsonModal != null)
        {
            antiCheatJsonModal.SetActive(false);
        }
    }

    public void ShowModal()
    {
        if (antiCheatJsonModal != null)
        {
            antiCheatJsonModal.SetActive(true);

            Time.timeScale = 0f;
        }
    }

    public void OnResetGameClicked()
    {
        Time.timeScale = 1f;

        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}
