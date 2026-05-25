using TMPro;
using UnityEngine;

public class System_WordsEffect : MonoBehaviour
{
    [SerializeField] GameObject textPrefab;
    [SerializeField] Transform container;
    [SerializeField] string[] waterWords = { "Plunk", "Bloop", "Splish", "Blop", "Plip-plop", "Splash!", "Gush!", "Drip" };
    [SerializeField] Color normalColor = Color.white;

    [Header("Dynamic Color Variations:")]
    [SerializeField]
    Color[] waterColorPalette = new Color[]
    {
        new Color(0.0f, 0.75f, 1.0f, 1.0f),
        new Color(0.0f, 1.0f, 1.0f, 1.0f),
        new Color(0.4f, 0.8f, 1.0f, 1.0f),
        new Color(0.1f, 0.95f, 0.7f, 1.0f)
    };

    RectTransform containerRect;

    void Awake()
    {
        if (container != null)
        {
            containerRect = container.GetComponent<RectTransform>();
        }
    }

    public void ShowRandomWord()
    {
        string word = waterWords[Random.Range(0, waterWords.Length)];
        Color randomWaterColor = waterColorPalette[Random.Range(0, waterColorPalette.Length)];

        SpawnText(word, randomWaterColor, 1.2f);
    }

    public void SpawnText(string text, Color color, float duration)
    {
        if (textPrefab == null || container == null || containerRect == null) return;

        GameObject newText = Instantiate(textPrefab, container);
        RectTransform rect = newText.GetComponent<RectTransform>();

        if (rect != null)
        {
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);

            float widthLimit = (containerRect.rect.width - 150f) / 2f;
            float heightLimit = (containerRect.rect.height - 150f) / 2f;

            float randomX = Random.Range(-widthLimit, widthLimit);
            float randomY = Random.Range(-heightLimit, heightLimit);

            rect.anchoredPosition = new Vector2(randomX, randomY);
            rect.transform.localScale = Vector3.one;
            rect.transform.localPosition = new Vector3(randomX, randomY, 0f);
        }

        var anim = newText.GetComponent<Animation_WordEffect>();
        if (anim != null)
        {
            anim.Setup(text, color, duration);
        }
    }
}