using TMPro;
using UnityEngine;

public class FloatingBonusText : MonoBehaviour
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

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        textComponent = GetComponentInChildren<TextMeshProUGUI>();

        if (textComponent == null)
            textComponent = GetComponent<TextMeshProUGUI>();
    }

    public void Initialize(string textValue)
    {
        if (textComponent != null)
        {
            textComponent.text = $"+{textValue}";
        }

        startPosition = rectTransform.anchoredPosition;
        chosenXOffset = Random.Range(randomXOffset.x, randomXOffset.y);

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        timer += Time.deltaTime;
        float progress = timer / lifeTime;
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