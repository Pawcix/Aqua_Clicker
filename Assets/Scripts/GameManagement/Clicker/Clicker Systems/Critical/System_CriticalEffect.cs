using TMPro;
using UnityEngine;
using System.Collections;

public class System_CriticalEffect : MonoBehaviour
{
    [SerializeField] System_WordsEffect wordsEffect;
    [SerializeField] string[] critWords = { "CRITICAL!", "LUCKY!", "X5!", "BOOM!" };
    [SerializeField] Color critColor = Color.yellow;

    public void ShowCritEffect()
    {
        string word = critWords[Random.Range(0, critWords.Length)];
        wordsEffect.SpawnText(word, critColor, 1.2f);
    }
}
