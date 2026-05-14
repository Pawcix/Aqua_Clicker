using TMPro;
using UnityEngine;
using System.Collections;

public class GoldenRain : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] System_Data data;
    [SerializeField] GoldenDrop_Spawner spawner;
    [SerializeField] GameObject goldenRainIconPrefab;

    [Header("Event Settings:")]
    [SerializeField] float timeBetweenEvents = 300f;
    [SerializeField] float eventDuration = 10f;
    [SerializeField] float visibilityThreshold = 15f;

    GameObject activeIconInstance;
    TextMeshProUGUI timerTextInIcon;
    float currentEventTimer;
    bool isEventActive = false;

    void Start()
    {
        if (data.goldenRainTimer <= 0 && !isEventActive)
            data.goldenRainTimer = timeBetweenEvents;
    }

    void Update()
    {
        if (!isEventActive)
        {
            if (data.goldenRainTimer > 0)
            {
                data.goldenRainTimer -= Time.deltaTime;
                HandleWaitingStatus();
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
                UpdateIconTimerUI(currentEventTimer);
            }
            else
            {
                EndGoldenRain();
            }
        }
    }

    void HandleWaitingStatus()
    {
        if (data.goldenRainTimer <= visibilityThreshold)
        {
            if (activeIconInstance == null)
            {
                SpawnEventIcon();
            }
            UpdateIconTimerUI(data.goldenRainTimer);
        }
    }

    void SpawnEventIcon()
    {
        activeIconInstance = Event_Manager.Instance.AddEventIcon(goldenRainIconPrefab);
        if (activeIconInstance != null)
        {
            timerTextInIcon = activeIconInstance.GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    void UpdateIconTimerUI(float timeToShow)
    {
        if (timerTextInIcon == null) return;

        int minutes = Mathf.FloorToInt(timeToShow / 60);
        int seconds = Mathf.FloorToInt(timeToShow % 60);

        timerTextInIcon.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerTextInIcon.color = isEventActive ? Color.cyan : Color.white;
    }

    IEnumerator StartGoldenRain()
    {
        isEventActive = true;
        currentEventTimer = eventDuration;

        if (activeIconInstance == null) SpawnEventIcon();

        spawner.SetRainMode(true);

        yield return null;
    }

    void EndGoldenRain()
    {
        isEventActive = false;
        spawner.SetRainMode(false);
        data.goldenRainTimer = timeBetweenEvents;

        if (activeIconInstance != null)
        {
            Event_Manager.Instance.RemoveEventIcon(activeIconInstance);
            activeIconInstance = null;
            timerTextInIcon = null;
        }
    }
}