using TMPro;
using UnityEngine;
using System.Collections;

public class GoldenRain : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] System_Data data;
    [SerializeField] GoldenDrop_Spawner spawner;
    [SerializeField] TextMeshProUGUI statusText;

    [Header("Event Settings:")]
    [SerializeField] float timeBetweenEvents = 300f;
    [SerializeField] float eventDuration = 10f;

    float currentEventTimer;
    bool isEventActive = false;

    void Update()
    {
        if (!isEventActive)
        {
            if (data.goldenRainTimer > 0)
            {
                data.goldenRainTimer -= Time.deltaTime;
                UpdateWaitingUI();
            }
            else
            {
                StartCoroutine(StartGoldenRain());
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

        int minutes = Mathf.FloorToInt(data.goldenRainTimer / 60);
        int seconds = Mathf.FloorToInt(data.goldenRainTimer % 60);

        statusText.color = Color.white;
        statusText.text = string.Format("Golden Rain in: {0:00}:{1:00}", minutes, seconds);
    }

    void UpdateActiveEventUI()
    {
        if (statusText == null) return;

        statusText.color = Color.yellow;
        statusText.text = string.Format("GOLDEN RAIN: {0:0.0}s", currentEventTimer);
    }

    IEnumerator StartGoldenRain()
    {
        isEventActive = true;
        currentEventTimer = eventDuration;

        spawner.SetRainMode(true);

        yield return new WaitForSeconds(eventDuration);

        spawner.SetRainMode(false);
        data.goldenRainTimer = timeBetweenEvents;
        isEventActive = false;
    }
}
