using TMPro;
using UnityEngine;
using System.Collections;

public class GoldenRush : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] System_Data data;
    [SerializeField] GameObject goldRushIconPrefab;

    [Header("Event Settings:")]
    [SerializeField] float timeBetweenEvents = 400f;
    [SerializeField] float eventDuration = 30f;
    [SerializeField] float visibilityThreshold = 300f;

    GameObject activeIconInstance;
    TextMeshProUGUI timerTextInIcon;
    float currentEventTimer;
    bool isEventActive = false;

    void Start()
    {
        if (data.goldRushTimer <= 0 && !data.isGoldRushActive)
            data.goldRushTimer = timeBetweenEvents;
    }

    void Update()
    {
        if (!isEventActive)
        {
            if (data.goldRushTimer > 0)
            {
                data.goldRushTimer -= Time.deltaTime;
                HandleWaitingStatus();
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
                UpdateIconTimerUI(currentEventTimer);
            }
            else
            {
                EndGoldRush();
            }
        }
    }

    void HandleWaitingStatus()
    {
        if (data.goldRushTimer <= visibilityThreshold)
        {
            if (activeIconInstance == null)
            {
                SpawnEventIcon();
            }
            UpdateIconTimerUI(data.goldRushTimer);
        }
    }

    void SpawnEventIcon()
    {
        activeIconInstance = Event_Manager.Instance.AddEventIcon(goldRushIconPrefab);
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
    }

    IEnumerator StartGoldRush()
    {
        isEventActive = true;
        data.isGoldRushActive = true;
        currentEventTimer = eventDuration;

        if (activeIconInstance == null) SpawnEventIcon();

        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("GoldRushStart");

        yield return null;
    }

    void EndGoldRush()
    {
        isEventActive = false;
        data.isGoldRushActive = false;
        data.goldRushTimer = timeBetweenEvents;

        if (activeIconInstance != null)
        {
            Event_Manager.Instance.RemoveEventIcon(activeIconInstance);
            activeIconInstance = null;
            timerTextInIcon = null;
        }

        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("GoldRushEnd");
    }
}