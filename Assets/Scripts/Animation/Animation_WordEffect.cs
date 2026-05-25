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

    [Header("Juicy Physics Settings:")]
    [SerializeField] Vector2 horizontalVelocityRange = new Vector2(-60f, 60f);
    [SerializeField] float rotationSpeed = 120f;
    [SerializeField] float gravityModifier = 2.0f;

    float chosenHorizontalVelocity;
    float chosenRotationDirection;

    public void Setup(string text, Color color, float duration)
    {
        if (tmpText == null) return;

        tmpText.text = text;
        tmpText.color = color;
        floatDuration = duration;

        transform.SetAsLastSibling();
        transform.localScale = Vector3.one * startScale;

        chosenHorizontalVelocity = Random.Range(horizontalVelocityRange.x, horizontalVelocityRange.y);
        chosenRotationDirection = Random.Range(-1f, 1f);

        if (rectTransform != null)
        {
            rectTransform.localRotation = Quaternion.Euler(0f, 0f, Random.Range(-20f, 20f));
        }

        StartCoroutine(AnimateRoutine());
    }

    IEnumerator AnimateRoutine()
    {
        float elapsed = 0f;
        Color originalColor = tmpText.color;

        Vector2 startPosition = Vector2.zero;
        if (rectTransform != null)
        {
            startPosition = rectTransform.anchoredPosition;
        }

        float initialHorizontalThrust = (chosenHorizontalVelocity * 0.1f) / floatDuration;
        float internalGravityForce = gravityModifier / floatDuration;

        while (elapsed < floatDuration)
        {
            float t = elapsed / floatDuration;

            if (rectTransform != null)
            {
                float currentX = startPosition.x;
                float currentY = startPosition.y;

                currentX += (t * (chosenHorizontalVelocity + initialHorizontalThrust)) * (1f - (t * t * 0.7f));

                currentY += (moveSpeed * t) * (1f - (t * t * internalGravityForce));

                rectTransform.anchoredPosition = new Vector2(currentX, currentY);
                rectTransform.localRotation *= Quaternion.Euler(0f, 0f, chosenRotationDirection * rotationSpeed * Time.deltaTime);
            }

            float currentScale;
            if (t < 0.15f)
            {
                currentScale = Mathf.Lerp(startScale, maxScale * 1.05f, t / 0.15f);
            }
            else
            {
                currentScale = Mathf.Lerp(maxScale * 1.05f, 0f, (t - 0.15f) / 0.85f);
            }

            transform.localScale = Vector3.one * currentScale;

            float alpha = (t < 0.4f) ? 1f : Mathf.Lerp(1f, 0f, (t - 0.4f) / 0.6f);
            tmpText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}