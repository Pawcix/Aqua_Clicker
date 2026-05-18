using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class System_RebirthJuice : MonoBehaviour
{
    public static System_RebirthJuice Instance { get; private set; }

    [Header("Flash Settings:")]
    [SerializeField] Image flashImage;
    [SerializeField] Color flashColor = new Color(0.73f, 0.33f, 0.83f); 
    [SerializeField][Range(0f, 1f)] float maxAlpha = 0.7f;
    [SerializeField] float flashDuration = 0.5f;

    [Header("Popup Text Settings:")]
    [SerializeField] TextMeshProUGUI bigPopupText;
    [SerializeField] float textDuration = 2.0f;

    Coroutine currentFlashRoutine;
    Coroutine currentTextRoutine;

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

        if (flashImage != null)
        {
            flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0f);
            flashImage.gameObject.SetActive(false);
        }

        if (bigPopupText != null)
        {
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
        float halfDuration = flashDuration / 2f;
        float elapsed = 0f;

        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, maxAlpha, elapsed / halfDuration);
            flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            yield return null;
        }

        elapsed = 0f;

        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(maxAlpha, 0f, elapsed / halfDuration);
            flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            yield return null;
        }

        flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0f);
        flashImage.gameObject.SetActive(false);
    }

    IEnumerator TextPopupRoutine(float multiplier)
    {
        bigPopupText.text = $"<color=#BA55D3><size=130%>REBIRTH!</size></color>\n<size=85%>GLOBAL MULTIPLIER: {multiplier:F2}x</size>";
        bigPopupText.gameObject.SetActive(true);

        bigPopupText.transform.localScale = Vector3.zero;
        float elapsed = 0f;
        float scaleDuration = 0.15f;

        while (elapsed < scaleDuration)
        {
            elapsed += Time.deltaTime;
            bigPopupText.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, elapsed / scaleDuration);
            yield return null;
        }
        bigPopupText.transform.localScale = Vector3.one;

        yield return new WaitForSeconds(textDuration - scaleDuration);

        bigPopupText.gameObject.SetActive(false);
    }
}