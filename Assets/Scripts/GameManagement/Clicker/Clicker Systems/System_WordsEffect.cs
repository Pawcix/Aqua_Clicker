using TMPro;
using UnityEngine;
using System.Collections;

public class System_WordsEffect : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI effectText;
    [SerializeField] string[] waterWords = { "Plunk", "Bloop", "Splish", "Blop", "Plip-plop" };
    [SerializeField] float displayDuration = 0.25f;

    void Start()
    {
        if (effectText != null)
        {
            effectText.text = "";
            effectText.raycastTarget = false;
        }
    }

    public void ShowRandomWord()
    {
        if (effectText == null) return;

        string randomWord = waterWords[Random.Range(0, waterWords.Length)];
        effectText.text = randomWord;

        float rotationZ = Random.Range(-30f, 30f);
        effectText.transform.rotation = Quaternion.Euler(0, 0, rotationZ);

        RectTransform rect = effectText.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(Random.Range(-80, 80), Random.Range(-130, 130));

        StopAllCoroutines();
        StartCoroutine(ResetTextAfterDelay(displayDuration));
    }

    IEnumerator ResetTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        effectText.text = "";
        effectText.transform.rotation = Quaternion.identity;
    }
}
