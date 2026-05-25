using TMPro;
using UnityEngine;

public class System_CriticalEffect : MonoBehaviour
{
    [SerializeField] System_WordsEffect wordsEffect;

    [Header("Epic Critical Dictionary:")]
    [SerializeField]
    string[] critWords = {
        "CRITICAL!", "TSUNAMI!", "TORRENT!", "MEGA SPLASH!", "BURST!", "SURGE!"
    };

    [SerializeField] Color critColor = new Color(0.91f, 0.31f, 0.25f, 1.0f);

    public void ShowCritEffect()
    {
        if (wordsEffect == null) return;

        string word = critWords[Random.Range(0, critWords.Length)];

        string formattedCritWord = $"<size=140%><b>{word}</b></size>";

        wordsEffect.SpawnText(formattedCritWord, critColor, 1.1f);
    }
}