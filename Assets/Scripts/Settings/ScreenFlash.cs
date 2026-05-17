using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoBehaviour
{
    public static ScreenFlash Instance { get; private set; }

    [Header("Flash Settings:")]
    [SerializeField] Image flashImage;
    [SerializeField] Color flashColor = new Color(1f, 0.84f, 0f);
    [SerializeField][Range(0f, 1f)] float maxAlpha = 0.5f;
    [SerializeField] float flashDuration = 0.3f;

    Coroutine currentFlashRoutine;

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
    }

    public void TriggerGoldFlash()
    {
        if (flashImage == null) return;

        if (currentFlashRoutine != null)
        {
            StopCoroutine(currentFlashRoutine);
        }

        currentFlashRoutine = StartCoroutine(FlashRoutine());
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
}
