using TMPro;
using UnityEngine;
using System.Collections;

public class System_WordBoxText : MonoBehaviour
{
    [Header("Settings:")]
    [SerializeField] float moveSpeed = 80f;
    [SerializeField] float floatDuration = 0.5f;

    TextMeshProUGUI tmpText;
    RectTransform rectTransform;

    void Awake()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void Setup(string text, Color color, float duration)
    {
        if (tmpText == null) tmpText = GetComponent<TextMeshProUGUI>();

        tmpText.text = text;
        tmpText.color = color;

        Color startColor = tmpText.color;
        startColor.a = 1f;
        tmpText.color = startColor;
        floatDuration = duration;
        StartCoroutine(FloatAndFadeRoutine());
    }

    IEnumerator FloatAndFadeRoutine()
    {
        float elapsed = 0f;
        Color originalColor = tmpText.color;
        Vector2 startPosition = rectTransform.anchoredPosition;

        while (elapsed < floatDuration)
        {

            rectTransform.anchoredPosition = startPosition + (Vector2.up * moveSpeed * elapsed);

            float alpha = Mathf.Lerp(1f, 0f, elapsed / floatDuration);
            tmpText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
