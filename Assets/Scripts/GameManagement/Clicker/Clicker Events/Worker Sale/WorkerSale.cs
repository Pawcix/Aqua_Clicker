using TMPro;
using UnityEngine;
using System.Collections;

public class WorkerSale : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] System_Data data;
    [SerializeField] GameObject saleIconPrefab;

    [Header("Event Settings:")]
    [SerializeField] float minTimeBetweenEvents = 400f;
    [SerializeField] float maxTimeBetweenEvents = 800f;
    [SerializeField] float eventDuration = 10f;

    GameObject activeIconInstance;
    TextMeshProUGUI timerTextInIcon;
    float currentEventTimer;
    bool isEventActive = false;

    void Start()
    {
        if (data.workerSaleTimer <= 0)
            SetRandomTimer();
    }

    void Update()
    {
        if (!isEventActive)
        {
            if (data.workerSaleTimer > 0)
            {
                data.workerSaleTimer -= Time.deltaTime;
            }
            else
            {
                StartCoroutine(StartSaleRoutine());
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
                EndSale();
            }
        }
    }

    void SpawnEventIcon()
    {
        activeIconInstance = Event_Manager.Instance.AddEventIcon(saleIconPrefab);
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
        timerTextInIcon.color = Color.white;
    }

    IEnumerator StartSaleRoutine()
    {
        isEventActive = true;
        data.isWorkerSaleActive = true;
        currentEventTimer = eventDuration;

        if (activeIconInstance == null) SpawnEventIcon();

        RefreshAllWorkerUI();
        yield return null;
    }

    void EndSale()
    {
        isEventActive = false;
        data.isWorkerSaleActive = false;

        if (activeIconInstance != null)
        {
            Event_Manager.Instance.RemoveEventIcon(activeIconInstance);
            activeIconInstance = null;
            timerTextInIcon = null;
        }

        RefreshAllWorkerUI();
        SetRandomTimer();
    }

    void SetRandomTimer()
    {
        data.workerSaleTimer = Random.Range(minTimeBetweenEvents, maxTimeBetweenEvents);
    }

    void RefreshAllWorkerUI()
    {
        Worker_Element[] allWorkers = Object.FindObjectsByType<Worker_Element>(FindObjectsInactive.Exclude);

        foreach (Worker_Element worker in allWorkers)
        {
            worker.UpdateUI();
        }
    }
}