using TMPro;
using UnityEngine;

public class FloatingComboRewardText : MonoBehaviour
{
    [Header("Movement Settings:")]
    [SerializeField] float lifeTime = 1.2f;
    [SerializeField] float floatSpeed = 80f;
    [SerializeField] Vector2 randomXOffset = new Vector2(-20f, 20f);

    [Header("Juice Curves:")]
    [SerializeField] AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] AnimationCurve alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

    RectTransform rectTransform;
    TextMeshProUGUI textComponent;
    float timer = 0f;
    Vector3 startPosition;
    float chosenXOffset;
    Color originalColor;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        textComponent = GetComponentInChildren<TextMeshProUGUI>();

        if (textComponent == null)
            textComponent = GetComponent<TextMeshProUGUI>();

        if (textComponent != null)
        {
            Color col = textComponent.color;
            originalColor = new Color(col.r, col.g, col.b, 1f);
            textComponent.color = originalColor;
        }
    }

    public void Setup(string formattedValue)
    {
        if (textComponent == null || rectTransform == null) return;

        textComponent.text = $"+{formattedValue}";

        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.localPosition = Vector3.zero;

        startPosition = rectTransform.anchoredPosition;
        chosenXOffset = Random.Range(randomXOffset.x, randomXOffset.y);

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        timer += Time.deltaTime;
        float progress = timer / lifeTime;

        if (progress > 1f) return;

        float newY = startPosition.y + (progress * floatSpeed);
        float newX = startPosition.x + (progress * chosenXOffset);
        rectTransform.anchoredPosition = new Vector2(newX, newY);

        float currentScale = scaleCurve.Evaluate(progress);
        rectTransform.localScale = new Vector3(currentScale, currentScale, 1f);

        if (textComponent != null)
        {
            float currentAlpha = alphaCurve.Evaluate(progress);
            textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, currentAlpha);
        }
    }
}