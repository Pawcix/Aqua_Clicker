using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RiskReward : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] System_Data data;
    [SerializeField] GameObject boosterIconPrefab;

    [Header("Dynamic Icons (Visuals):")]
    [SerializeField] Sprite winIconSprite;
    [SerializeField] Sprite loseIconSprite;

    GameObject activeIconInstance;
    TextMeshProUGUI timerTextInIcon;
    Image iconDisplayInIcon;

    void Update()
    {
        if (data.riskBonusTimer > 0 && data.riskMultiplier == 1.0f)
        {
            RemoveBoosterIcon();
            return;
        }

        if (data.riskBonusTimer > 0)
        {
            data.riskBonusTimer -= Time.deltaTime;

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

            Transform iconTransform = activeIconInstance.transform.Find("Img - Risk Reward");
            if (iconTransform != null)
            {
                iconDisplayInIcon = iconTransform.GetComponent<Image>();

                if (iconDisplayInIcon != null)
                {
                    if (data.riskMultiplier > 1.0f)
                    {
                        iconDisplayInIcon.sprite = winIconSprite;
                    }
                    else
                    {
                        iconDisplayInIcon.sprite = loseIconSprite;
                    }
                }
            }
        }
    }

    void UpdateIconUI()
    {
        if (timerTextInIcon == null) return;

        int mins = Mathf.FloorToInt(data.riskBonusTimer / 60f);
        int secs = Mathf.FloorToInt(data.riskBonusTimer % 60f);

        if (data.riskBonusTimer < 0) { mins = 0; secs = 0; }

        timerTextInIcon.text = string.Format("{0:D2}:{1:D2}", mins, secs);
    }

    void RemoveBoosterIcon()
    {
        if (Event_Manager.Instance != null && activeIconInstance != null)
        {
            Event_Manager.Instance.RemoveEventIcon(activeIconInstance);
        }

        activeIconInstance = null;
        timerTextInIcon = null;
        iconDisplayInIcon = null;

        if (data != null)
        {
            data.riskMultiplier = 1.0f;
            data.riskBonusTimer = 0;
        }
    }
}
