using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [Header("Settings:")]
    [SerializeField] string sceneToLoad = "Scene_Game";

    [Header("UI Elements:")]
    [SerializeField] Image progressBar;
    [SerializeField] TextMeshProUGUI progressText;

    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        yield return new WaitForSeconds(0.2f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (progressBar != null)
                progressBar.fillAmount = progress;

            if (progressText != null)
                progressText.text = $"Loading: {(progress * 100):F0}%";

            yield return null;
        }
    }
}
