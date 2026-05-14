using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WheelRewards : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] System_Data data;
    [SerializeField] GameObject boosterIconPrefab;

    GameObject activeIconInstance;
    TextMeshProUGUI timerTextInIcon;
    Image iconDisplayInIcon;

    void Update()
    {
        if (data.wheelBonusTimer > 0)
        {
            data.wheelBonusTimer -= Time.deltaTime;

            if (activeIconInstance == null)
            {
                SpawnBoosterIcon();
            }

            UpdateIconUI();
        }
        else
        {
            if (activeIconInstance != null)
            {
                RemoveBoosterIcon();
            }
        }
    }

    void SpawnBoosterIcon()
    {
        activeIconInstance = Event_Manager.Instance.AddEventIcon(boosterIconPrefab);

        if (activeIconInstance != null)
        {
            activeIconInstance.transform.SetAsFirstSibling();
            timerTextInIcon = activeIconInstance.GetComponentInChildren<TextMeshProUGUI>();

            Transform iconTransform = activeIconInstance.transform.Find("Img - Wheel Reward");
            if (iconTransform != null)
            {
                iconDisplayInIcon = iconTransform.GetComponent<Image>();

                if (iconDisplayInIcon != null && data.currentWheelRewardIcon != null)
                {
                    iconDisplayInIcon.sprite = data.currentWheelRewardIcon;
                }
            }
        }
    }

    void UpdateIconUI()
    {
        if (timerTextInIcon == null) return;

        int mins = Mathf.FloorToInt(data.wheelBonusTimer / 60f);
        int secs = Mathf.FloorToInt(data.wheelBonusTimer % 60f);

        if (data.wheelBonusTimer < 0) { mins = 0; secs = 0; }

        timerTextInIcon.text = string.Format("{0:D2}:{1:D2}", mins, secs);
    }

    void RemoveBoosterIcon()
    {
        Event_Manager.Instance.RemoveEventIcon(activeIconInstance);
        activeIconInstance = null;
        timerTextInIcon = null;
        iconDisplayInIcon = null;
        data.wheelMultiplier = 1.0f;
        data.wheelBonusTimer = 0;
        data.currentWheelRewardIcon = null;
    }
}