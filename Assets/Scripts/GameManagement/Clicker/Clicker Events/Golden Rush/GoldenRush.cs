using TMPro;
using UnityEngine;
using System.Collections;

public class GoldenRush : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] System_Data data;
    [SerializeField] TextMeshProUGUI statusText;

    [Header("Event Settings:")]
    [SerializeField] float timeBetweenEvents = 450f;
    [SerializeField] float eventDuration = 15f;

    float currentEventTimer;
    bool isEventActive = false;

    void Update()
    {
        if (!isEventActive)
        {
            if (data.goldRushTimer > 0)
            {
                data.goldRushTimer -= Time.deltaTime;
                UpdateWaitingUI();
            }
            else
            {
                StartCoroutine(StartGoldRush());
            }
        }
        else
        {
            if (currentEventTimer > 0)
            {
                currentEventTimer -= Time.deltaTime;
                UpdateActiveEventUI();
            }
        }
    }

    void UpdateWaitingUI()
    {
        if (statusText == null) return;
        int minutes = Mathf.FloorToInt(data.goldRushTimer / 60);
        int seconds = Mathf.FloorToInt(data.goldRushTimer % 60);
        statusText.color = Color.white;
        statusText.text = string.Format("Gold Rush in: \n {0:00}:{1:00}", minutes, seconds);
    }

    void UpdateActiveEventUI()
    {
        if (statusText == null) return;
        statusText.color = new Color(1f, 0.84f, 0f);
        statusText.text = string.Format("GOLD RUSH x2: \n {0:0.0}s", currentEventTimer);
    }

    IEnumerator StartGoldRush()
    {
        isEventActive = true;
        data.isGoldRushActive = true;
        currentEventTimer = eventDuration;

        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("GoldRushStart");

        yield return new WaitForSeconds(eventDuration);

        data.isGoldRushActive = false;
        data.goldRushTimer = timeBetweenEvents;
        isEventActive = false;
    }
}
