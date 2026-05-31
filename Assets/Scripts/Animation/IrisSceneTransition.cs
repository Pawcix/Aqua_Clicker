using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]
public class IrisMaskController : MonoBehaviour
{
    public static IrisMaskController Instance;

    [Header("Settings:")]
    public string targetScene = "Scene_Loading";
    public float fadeDuration = 0.5f;

    [Header("Animation Settings:")]
    [SerializeField] RectTransform maskRect;
    [SerializeField] float maxScale = 2f;
    [SerializeField] float minScale = 0f;

    CanvasGroup canvasGroup;
    bool isFading = false;

    void Awake()
    {
        Instance = this;
        canvasGroup = GetComponent<CanvasGroup>();

        if (maskRect == null)
        {
            maskRect = GetComponent<RectTransform>();
        }
    }

    void Start()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;

            StartCoroutine(FadeInRoutine());
        }
    }

    IEnumerator FadeInRoutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;

            canvasGroup.alpha = Mathf.Clamp01(1f - t);

            if (maskRect != null)
            {
                float currentScale = Mathf.Lerp(maxScale, 1f, t);
                maskRect.localScale = new Vector3(currentScale, currentScale, 1f);

                float currentAngle = Mathf.Lerp(360f, 0f, t);
                maskRect.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
            }

            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;

        if (maskRect != null)
        {
            maskRect.localScale = Vector3.one;
            maskRect.localRotation = Quaternion.identity;
        }
    }

    public void StartFadeOut()
    {
        if (isFading) return;
        isFading = true;

        canvasGroup.blocksRaycasts = true;
        StartCoroutine(FadeOutRoutine());
    }

    IEnumerator FadeOutRoutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;

            canvasGroup.alpha = Mathf.Clamp01(t);

            if (maskRect != null)
            {
                float currentScale = Mathf.Lerp(1f, minScale, t);
                maskRect.localScale = new Vector3(currentScale, currentScale, 1f);

                float currentAngle = Mathf.Lerp(0f, -360f, t);
                maskRect.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
            }

            yield return null;
        }

        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(targetScene);
    }
}