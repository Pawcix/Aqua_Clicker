using TMPro;
using UnityEngine;

public class System_CPS : MonoBehaviour
{
    [Header("UI Elements:")]
    [SerializeField] GameObject cpsObject;
    [SerializeField] TextMeshProUGUI cpsText;

    [Header("Settings:")]
    [SerializeField] float hideDelay = 3f;

    int clickCount = 0;
    int currentCPS = 0;
    float resetTimer = 0f;
    float hideTimer = 0f;
    bool isVisible = false;

    void Start()
    {
        if (cpsObject != null) cpsObject.SetActive(false);
    }

    public void OnClickRegistered()
    {
        clickCount++;
        hideTimer = 0f;

        if (!isVisible)
        {
            isVisible = true;
            cpsObject.SetActive(true);
        }
    }

    void Update()
    {
        if (!isVisible) return;

        resetTimer += Time.deltaTime;
        if (resetTimer >= 1f)
        {
            currentCPS = clickCount;
            clickCount = 0;
            resetTimer = 0f;
            UpdateDisplay();
        }

        hideTimer += Time.deltaTime;
        if (hideTimer >= hideDelay)
        {
            HideCPS();
        }
    }

    void UpdateDisplay()
    {
        if (cpsText != null)
        {
            cpsText.text = $"CPS: {currentCPS}";
        }
    }

    void HideCPS()
    {
        isVisible = false;
        currentCPS = 0;
        clickCount = 0;
        if (cpsObject != null) cpsObject.SetActive(false);
    }
}
