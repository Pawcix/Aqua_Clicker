using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GoldenDrop_Item : MonoBehaviour
{
    [Header("Settings:")]
    [SerializeField] float fallSpeed = 300f;
    [SerializeField] GameObject bonusTextPrefab;

    [Header("Organic Movement (Water Drop Physics):")]
    [SerializeField] float sideSwayAmplitude = 40f;
    [SerializeField] float sideSwayFrequency = 3f;
    [SerializeField] float speedFluctuation = 100f;

    [Header("Juicy Click Animation Settings:")]
    [SerializeField] Image dropImage;
    [SerializeField] Sprite shockwaveSprite;
    [SerializeField] Color shockwaveColor = new Color(1f, 0.85f, 0f, 0.6f);
    [SerializeField] float animationDuration = 0.2f;

    System_Data data;
    RectTransform rectTransform;

    float existenceTime = 0f;
    public float ExistenceTime => existenceTime;
    bool wasCollected = false;

    float randomOffset;
    float customSwayAmp;
    float customSwayFreq;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (dropImage == null) dropImage = GetComponent<Image>();

        randomOffset = Random.Range(0f, 100f);
        customSwayAmp = Random.Range(sideSwayAmplitude * 0.5f, sideSwayAmplitude * 1.5f);
        customSwayFreq = Random.Range(sideSwayFrequency * 0.7f, sideSwayFrequency * 1.3f);
    }
    void Start()
    {
        data = Object.FindAnyObjectByType<System_Data>();

        Button btn = GetComponent<Button>();
        if (btn != null) btn.onClick.AddListener(OnGoldenDropClicked);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("Golden Drop - Fall");
        }

        Destroy(gameObject, 12f);
    }

    void Update()
    {
        if (wasCollected || rectTransform == null) return;

        existenceTime += Time.deltaTime;
        float timeCombined = existenceTime * customSwayFreq + randomOffset;

        float currentSpeed = fallSpeed + (Mathf.Cos(timeCombined * 1.5f) * speedFluctuation);
        float deltaY = -currentSpeed * Time.deltaTime;

        float currentSwaySpeed = Mathf.Sin(timeCombined) * customSwayAmp * customSwayFreq;
        float deltaX = currentSwaySpeed * Time.deltaTime;

        rectTransform.anchoredPosition += new Vector2(deltaX, deltaY);
    }

    public void OnGoldenDropClicked()
    {
        if (data == null || wasCollected) return;
        wasCollected = true;

        double currentPoints = data.pointsCounterFloat;
        double bonus = currentPoints * 0.10;

        if (bonus < 100.0) bonus = 100.0;

        System_Economy.Instance.AddPoints(bonus);
        data.goldenDrops++;

        if (ScreenFlash.Instance != null) ScreenFlash.Instance.TriggerGoldFlash();

        PointsDisplay display = PointsDisplay.Instance != null ? PointsDisplay.Instance : Object.FindAnyObjectByType<PointsDisplay>();
        if (display != null) display.PulseGoldenDrop();

        Clicker_Prefabs prefabsManager = Object.FindAnyObjectByType<Clicker_Prefabs>();
        if (prefabsManager != null)
        {
            int displayPPS = (int)System.Math.Floor(data.pointsPerSecond);
            prefabsManager.UpdateAllPrefabs(data.pointsCounterFloat, displayPPS);
        }

        Vector2 collectPosition = rectTransform.anchoredPosition;
        SpawnBonusText(NumberFormatter.FormatWithDots(bonus), collectPosition);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("Golden Drop - Collect");
        }

        if (System_Leveling.Instance != null)
        {
            System_Leveling.Instance.FlashGoldenDropColor();
        }

        StartCoroutine(ClickAnimationRoutine());
    }

    IEnumerator ClickAnimationRoutine()
    {
        Button btn = GetComponent<Button>();
        if (btn != null) btn.interactable = false;

        GameObject shockwaveObj = null;
        RectTransform shockwaveRect = null;
        Image shockwaveImage = null;

        if (shockwaveSprite != null)
        {
            shockwaveObj = new GameObject("Shockwave_Effect", typeof(RectTransform), typeof(Image));
            shockwaveObj.transform.SetParent(transform.parent, false);

            shockwaveRect = shockwaveObj.GetComponent<RectTransform>();
            shockwaveRect.anchoredPosition = rectTransform.anchoredPosition;
            shockwaveRect.sizeDelta = rectTransform.sizeDelta;

            shockwaveImage = shockwaveObj.GetComponent<Image>();
            shockwaveImage.sprite = shockwaveSprite;
            shockwaveImage.color = shockwaveColor;
        }

        float elapsed = 0f;
        Vector3 initialScale = rectTransform.localScale;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animationDuration;

            float dropScaleMod = Mathf.Lerp(1f, 1.4f, t);
            if (t > 0.4f)
            {
                dropScaleMod = Mathf.Lerp(1.4f, 0f, (t - 0.4f) / 0.6f);
            }
            rectTransform.localScale = initialScale * dropScaleMod;

            if (shockwaveObj != null)
            {
                float waveScale = Mathf.Lerp(1f, 3.5f, t);
                shockwaveRect.localScale = new Vector3(waveScale, waveScale, 1f);

                Color c = shockwaveImage.color;
                c.a = Mathf.Lerp(shockwaveColor.a, 0f, t);
                shockwaveImage.color = c;
            }

            yield return null;
        }

        if (shockwaveObj != null) Destroy(shockwaveObj);
        Destroy(gameObject);
    }

    public void SetSpeed(float newSpeed)
    {
        fallSpeed = newSpeed;
    }

    void SpawnBonusText(string amountText, Vector2 spawnPos)
    {
        if (bonusTextPrefab == null) return;

        GameObject textObj = Instantiate(bonusTextPrefab, transform.parent);
        if (textObj != null)
        {
            GoldenDrop_FloatingText floatingTextScript = textObj.GetComponent<GoldenDrop_FloatingText>();
            if (floatingTextScript != null)
            {
                floatingTextScript.Initialize(amountText, spawnPos);
            }
            else
            {
                RectTransform textRect = textObj.GetComponent<RectTransform>();
                if (textRect != null) textRect.anchoredPosition = spawnPos;
                Destroy(textObj, 2f);
            }
        }
    }
}