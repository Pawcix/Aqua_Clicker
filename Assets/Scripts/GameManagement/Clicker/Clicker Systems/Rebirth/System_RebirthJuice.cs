using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class System_RebirthJuice : MonoBehaviour
{
    public static System_RebirthJuice Instance { get; set; }

    [Header("Flash Settings:")]
    [SerializeField] Image flashImage;
    [SerializeField] Color flashColor = new Color(0.73f, 0.33f, 0.83f);
    [SerializeField][Range(0f, 1f)] float maxAlpha = 0.7f;
    [SerializeField] float flashDuration = 0.5f;

    [Header("Popup Text Settings:")]
    [SerializeField] TextMeshProUGUI bigPopupText;
    [SerializeField] float textDuration = 2.5f;

    [Header("Juice Juice Juice (Advanced Animations):")]
    [SerializeField] float punchScale = 1.25f;
    [SerializeField] float idlePulseSpeed = 4f;
    [SerializeField] float idlePulseAmount = 0.05f;
    [SerializeField] float idleBobSpeed = 3f;
    [SerializeField] float idleBobAmount = 15f;
    [SerializeField] float fadeOutDuration = 0.4f;

    Coroutine currentFlashRoutine;
    Coroutine currentTextRoutine;
    Vector3 originalTextPosition;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Canvas canvas = GetComponent<Canvas>();
        if (canvas == null)
        {
            canvas = gameObject.AddComponent<Canvas>();
        }

        canvas.overrideSorting = true;
        canvas.sortingOrder = 100;

        if (GetComponent<GraphicRaycaster>() == null)
        {
            gameObject.AddComponent<GraphicRaycaster>();
        }

        if (flashImage != null)
        {
            flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0f);
            flashImage.gameObject.SetActive(false);
        }

        if (bigPopupText != null)
        {
            originalTextPosition = bigPopupText.transform.localPosition;
            bigPopupText.gameObject.SetActive(false);
        }
    }

    public void TriggerRebirthEffects(float finalMultiplier)
    {
        if (flashImage != null)
        {
            if (currentFlashRoutine != null) StopCoroutine(currentFlashRoutine);
            currentFlashRoutine = StartCoroutine(FlashRoutine());
        }

        if (bigPopupText != null)
        {
            if (currentTextRoutine != null) StopCoroutine(currentTextRoutine);
            currentTextRoutine = StartCoroutine(TextPopupRoutine(finalMultiplier));
        }
    }

    IEnumerator FlashRoutine()
    {
        flashImage.gameObject.SetActive(true);

        float attackDuration = flashDuration * 0.25f;
        float decayDuration = flashDuration * 0.75f;
        float elapsed = 0f;

        while (elapsed < attackDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / attackDuration;
            float alpha = Mathf.Lerp(0f, maxAlpha, Mathf.SmoothStep(0f, 1f, t));
            flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            yield return null;
        }

        elapsed = 0f;

        while (elapsed < decayDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / decayDuration;
            float alpha = Mathf.Lerp(maxAlpha, 0f, t * t);
            flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            yield return null;
        }

        flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0f);
        flashImage.gameObject.SetActive(false);
    }

    IEnumerator TextPopupRoutine(float multiplier)
    {
        bigPopupText.text = $"<color=#FFA500>REBIRTH!</color>\nGLOBAL MULTIPLIER\n<color=#FFA500>{multiplier:F2}x</color>";

        bigPopupText.color = new Color(bigPopupText.color.r, bigPopupText.color.g, bigPopupText.color.b, 1f);
        bigPopupText.transform.localPosition = originalTextPosition;
        bigPopupText.transform.localScale = Vector3.zero;
        bigPopupText.gameObject.SetActive(true);

        float elapsed = 0f;
        float introDuration = 0.25f;

        while (elapsed < introDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / introDuration;

            float currentScale = 0f;
            if (t < 0.6f)
            {
                currentScale = Mathf.Lerp(0f, punchScale, t / 0.6f);
            }
            else
            {
                currentScale = Mathf.Lerp(punchScale, 1.0f, (t - 0.6f) / 0.4f);
            }

            bigPopupText.transform.localScale = Vector3.one * currentScale;
            yield return null;
        }
        bigPopupText.transform.localScale = Vector3.one;

        float idleTimeRemaining = textDuration - introDuration - fadeOutDuration;
        float idleElapsed = 0f;

        while (idleElapsed < idleTimeRemaining)
        {
            idleElapsed += Time.deltaTime;

            float yOffset = Mathf.Sin(Time.time * idleBobSpeed) * idleBobAmount;
            bigPopupText.transform.localPosition = originalTextPosition + new Vector3(0f, yOffset, 0f);

            float pulse = 1.0f + Mathf.Sin(Time.time * idlePulseSpeed) * idlePulseAmount;
            bigPopupText.transform.localScale = Vector3.one * pulse;

            yield return null;
        }

        elapsed = 0f;
        Color startColor = bigPopupText.color;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeOutDuration;

            float newAlpha = Mathf.Lerp(1f, 0f, t);
            bigPopupText.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);

            bigPopupText.transform.localPosition += new Vector3(0f, Time.deltaTime * 40f, 0f);

            yield return null;
        }

        bigPopupText.gameObject.SetActive(false);
    }
}