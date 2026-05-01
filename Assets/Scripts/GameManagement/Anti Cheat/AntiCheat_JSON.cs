using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AntiCheat_JSON : MonoBehaviour
{
    public static AntiCheat_JSON Instance;

    [Header("UI Elements:")]
    [SerializeField] GameObject antiCheatPanel;
    [SerializeField] Button resetGameButton;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        if (antiCheatPanel != null) antiCheatPanel.SetActive(false);

        if (resetGameButton != null)
        {
            resetGameButton.onClick.RemoveAllListeners();
            resetGameButton.onClick.AddListener(OnResetRequested);
        }
    }

    public void TriggerFileTamperProtection()
    {
        if (antiCheatPanel != null)
        {
            antiCheatPanel.SetActive(true);

            Time.timeScale = 0f;

            // Debug.LogWarning("[AntiCheat] Wykryto manipulację w plikach lokalnych! Gra zablokowana.");
        }
    }

    void OnResetRequested()
    {
        Time.timeScale = 1f;

        string savePath = System.IO.Path.Combine(Application.persistentDataPath, "savegame.dat");
        if (System.IO.File.Exists(savePath))
        {
            System.IO.File.Delete(savePath);
        }

        Data_Reset resetScript = Object.FindFirstObjectByType<Data_Reset>();
        if (resetScript != null)
        {
            resetScript.ResetGame();
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}
