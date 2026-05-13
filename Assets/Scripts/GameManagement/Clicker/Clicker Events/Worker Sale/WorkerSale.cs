using TMPro;
using UnityEngine;
using System.Collections;

public class WorkerSale : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] System_Data data;
    [SerializeField] TextMeshProUGUI statusText;
    [SerializeField] TextMeshProUGUI bannerText;
    [SerializeField] GameObject eventUIBox;

    [Header("Event Settings:")]
    [SerializeField] float minTimeBetweenEvents = 400f;
    [SerializeField] float maxTimeBetweenEvents = 900f;
    [SerializeField] float eventDuration = 30f;
    [SerializeField] float visibilityThreshold = 10f;

    float currentEventTimer;
    bool isEventActive = false;

    void Awake()
    {
        if (bannerText != null) bannerText.gameObject.SetActive(false);
    }

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
                UpdateWaitingUI();
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
                UpdateActiveEventUI();
            }
            else
            {
                EndSale();
            }
        }
    }

    void UpdateWaitingUI()
    {
        if (eventUIBox == null) return;

        if (data.workerSaleTimer > visibilityThreshold)
        {
            if (eventUIBox.activeSelf) eventUIBox.SetActive(false);
        }
        else
        {
            if (!eventUIBox.activeSelf)
            {
                eventUIBox.SetActive(true);
                if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Countdown_Alert");
            }

            if (statusText != null)
            {
                statusText.color = Color.white;
                int minutes = Mathf.FloorToInt(data.workerSaleTimer / 60);
                int seconds = Mathf.FloorToInt(data.workerSaleTimer % 60);
                statusText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
        }
    }

    void UpdateActiveEventUI()
    {
        if (statusText == null) return;

        int minutes = Mathf.FloorToInt(currentEventTimer / 60);
        int seconds = Mathf.FloorToInt(currentEventTimer % 60);
        statusText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    IEnumerator StartSaleRoutine()
    {
        isEventActive = true;
        data.isWorkerSaleActive = true;
        currentEventTimer = eventDuration;

        if (eventUIBox != null) eventUIBox.SetActive(true);

        if (bannerText != null)
        {
            bannerText.text = "WORKER SALE -25%";
            bannerText.gameObject.SetActive(true);
        }

        RefreshAllWorkerUI();
        yield return null;
    }

    void EndSale()
    {
        isEventActive = false;
        data.isWorkerSaleActive = false;

        if (bannerText != null) bannerText.gameObject.SetActive(false);
        if (eventUIBox != null) eventUIBox.SetActive(false);

        RefreshAllWorkerUI();
        SetRandomTimer();
    }

    void SetRandomTimer()
    {
        data.workerSaleTimer = Random.Range(minTimeBetweenEvents, maxTimeBetweenEvents);
    }

    void RefreshAllWorkerUI()
    {
        Worker_Element[] allWorkers = Object.FindObjectsByType<Worker_Element>(FindObjectsSortMode.None);
        foreach (Worker_Element worker in allWorkers)
        {
            worker.UpdateUI();
        }
    }
}