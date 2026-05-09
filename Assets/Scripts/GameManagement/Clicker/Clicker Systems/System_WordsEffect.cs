using TMPro;
using UnityEngine;
using System.Collections;

public class System_WordsEffect : MonoBehaviour
{
    [SerializeField] GameObject textPrefab;
    [SerializeField] Transform container;
    [SerializeField] string[] waterWords = { "Plunk", "Bloop", "Splish", "Blop", "Plip-plop" };
    [SerializeField] Color normalColor = Color.white;

    public void ShowRandomWord()
    {
        string word = waterWords[Random.Range(0, waterWords.Length)];
        SpawnText(word, normalColor, 0.5f);
    }

    public void SpawnText(string text, Color color, float duration)
    {
        if (textPrefab == null || container == null) return;

        GameObject newText = Instantiate(textPrefab, container);
        RectTransform rect = newText.GetComponent<RectTransform>();

        if (rect != null)
        {
            rect.anchoredPosition = new Vector2(Random.Range(-120, 120), Random.Range(-100, 100));
            rect.localRotation = Quaternion.Euler(0, 0, Random.Range(-15f, 15f));
        }

        var anim = newText.GetComponent<Animation_WordEffect>();
        if (anim != null)
        {
            anim.Setup(text, color, duration);
        }
    }
}
