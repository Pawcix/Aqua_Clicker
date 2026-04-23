using UnityEngine;
using System.Collections;

public class Data_AutoSave : MonoBehaviour
{
    [SerializeField] GameObject savingText;
    [SerializeField] float interval = 60f;
    [SerializeField] float displayDuration = 2f;

    void Start()
    {
        if (savingText != null) savingText.SetActive(false);
        StartCoroutine(AutosaveRoutine());
    }

    IEnumerator AutosaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            if (Data_SaveManager.instance != null)
            {
                Data_SaveManager.instance.SaveGame();
                StartCoroutine(ShowFeedback());
            }
        }
    }

    IEnumerator ShowFeedback()
    {
        if (savingText != null)
        {
            savingText.SetActive(true);
            yield return new WaitForSeconds(displayDuration);
            savingText.SetActive(false);
        }
    }
}
