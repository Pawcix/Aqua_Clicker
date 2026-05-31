using TMPro;
using UnityEngine;

public class GoldenDrop_FloatingText : MonoBehaviour
{
    [Header("Movement Settings:")]
    [SerializeField] float lifeTime = 1.2f;
    [SerializeField] float floatSpeed = 120f;
    [SerializeField] Vector2 randomXOffset = new Vector2(-40f, 40f);

    [Header("Juice Curves (Pop & Fade):")]
    [SerializeField] AnimationCurve scaleCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] AnimationCurve alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

    RectTransform rectTransform;
    TextMeshProUGUI textComponent;
    float timer = 0f;
    Vector2 startPosition;
    float chosenXOffset;
    bool isInitialized = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        textComponent = GetComponentInChildren<TextMeshProUGUI>();

        if (textComponent == null)
        {
            textComponent = GetComponent<TextMeshProUGUI>();
        }

        if (scaleCurve.keys.Length <= 2)
        {
            scaleCurve = new AnimationCurve(
                new Keyframe(0f, 0f),
                new Keyframe(0.2f, 1.3f),
                new Keyframe(0.4f, 1f),
                new Keyframe(1f, 0.8f)
            );
        }
    }

    public void Initialize(string textValue, Vector2 spawnPos)
    {
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();

        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        rectTransform.anchoredPosition = spawnPos;
        startPosition = spawnPos;

        if (textComponent != null)
        {
            textComponent.text = $"+{textValue}";
        }

        chosenXOffset = Random.Range(randomXOffset.x, randomXOffset.y);

        rectTransform.localScale = Vector3.zero;

        isInitialized = true;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (!isInitialized) return;

        timer += Time.deltaTime;
        float progress = timer / lifeTime;

        if (progress > 1f) progress = 1f;

        float newY = startPosition.y + (progress * floatSpeed);
        float newX = startPosition.x + (progress * chosenXOffset);
        rectTransform.anchoredPosition = new Vector2(newX, newY);

        float currentScale = scaleCurve.Evaluate(progress);
        rectTransform.localScale = new Vector3(currentScale, currentScale, 1f);

        if (textComponent != null)
        {
            Color originalColor = textComponent.color;
            float currentAlpha = alphaCurve.Evaluate(progress);
            textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, currentAlpha);
        }
    }
}