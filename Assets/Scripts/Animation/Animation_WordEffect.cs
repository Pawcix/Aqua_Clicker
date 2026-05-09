using TMPro;
using UnityEngine;
using System.Collections;

public class Animation_WordEffect : MonoBehaviour
{
    [Header("UI Reference:")]
    [SerializeField] TextMeshProUGUI tmpText;
    [SerializeField] RectTransform rectTransform;

    [Header("Movement Settings:")]
    [SerializeField] float moveSpeed = 100f;
    [SerializeField] float floatDuration = 0.6f;

    [Header("Scale Settings:")]
    [SerializeField] float startScale = 0.5f;
    [SerializeField] float maxScale = 1.2f;

    public void Setup(string text, Color color, float duration)
    {
        if (tmpText == null)
        {
            Debug.LogError("Brakuje referencji do TextMeshPro na obiekcie: " + gameObject.name);
            return;
        }

        tmpText.text = text;
        tmpText.color = color;
        floatDuration = duration;

        StartCoroutine(AnimateRoutine());
    }

    IEnumerator AnimateRoutine()
    {
        float elapsed = 0f;
        Color originalColor = tmpText.color;
        Vector2 startPosition = rectTransform != null ? rectTransform.anchoredPosition : Vector2.zero;

        while (elapsed < floatDuration)
        {
            float t = elapsed / floatDuration;

            if (rectTransform != null)
                rectTransform.anchoredPosition = startPosition + (Vector2.up * moveSpeed * t);

            float currentScale = (t < 0.2f)
                ? Mathf.Lerp(startScale, maxScale, t / 0.2f)
                : Mathf.Lerp(maxScale, 0f, (t - 0.2f) / 0.8f);

            transform.localScale = Vector3.one * currentScale;

            float alpha = Mathf.Lerp(1f, 0f, t);
            tmpText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
