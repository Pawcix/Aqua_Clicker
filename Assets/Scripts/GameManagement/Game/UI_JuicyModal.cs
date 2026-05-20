using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UI_JuicyModal : MonoBehaviour
{
    [Header("Animation Settings:")]
    [SerializeField] float animationSpeed = 5f;
    [SerializeField]
    AnimationCurve openCurve = new AnimationCurve(
        new Keyframe(0f, 0f),
        new Keyframe(0.6f, 1.1f),
        new Keyframe(1f, 1f)
    );
    [SerializeField] AnimationCurve closeCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

    RectTransform rectTransform;
    CanvasGroup canvasGroup;
    Coroutine animationCoroutine;

    bool isClosing = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();

        isClosing = false;

        float startProgress = 0f;
        if (rectTransform.localScale.x > 0f && rectTransform.localScale.x < 1f)
        {
            startProgress = rectTransform.localScale.x;
        }
        else if (rectTransform.localScale.x >= 1f)
        {
            rectTransform.localScale = Vector3.zero;
            canvasGroup.alpha = 0f;
        }

        StopAnimation();
        animationCoroutine = StartCoroutine(AnimateOpen(startProgress));
    }

    public void CloseModal()
    {
        if (!gameObject.activeInHierarchy || isClosing) return;

        isClosing = true;

        float startProgress = 0f;
        if (rectTransform.localScale.x < 1.0f)
        {
            startProgress = 1f - rectTransform.localScale.x;
        }

        StopAnimation();
        animationCoroutine = StartCoroutine(AnimateClose(startProgress));
    }

    IEnumerator AnimateOpen(float startProgress)
    {
        float timer = startProgress;

        while (timer < 1f)
        {
            timer += Time.unscaledDeltaTime * animationSpeed;

            float scaleValue = openCurve.Evaluate(timer);
            rectTransform.localScale = new Vector3(scaleValue, scaleValue, 1f);

            canvasGroup.alpha = Mathf.Clamp01(timer * 2f);

            yield return null;
        }

        rectTransform.localScale = Vector3.one;
        canvasGroup.alpha = 1f;
    }

    IEnumerator AnimateClose(float startProgress)
    {
        float timer = startProgress;

        while (timer < 1f)
        {
            timer += Time.unscaledDeltaTime * animationSpeed;

            float scaleValue = closeCurve.Evaluate(timer);
            rectTransform.localScale = new Vector3(scaleValue, scaleValue, 1f);
            canvasGroup.alpha = Mathf.Clamp01(scaleValue);

            yield return null;
        }

        rectTransform.localScale = Vector3.zero;
        canvasGroup.alpha = 0f;
        isClosing = false;

        gameObject.SetActive(false);
    }

    void StopAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
    }
}