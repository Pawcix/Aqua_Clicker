using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class LuckyBonus : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] System_Data data;
    [SerializeField] GameObject bonusButtonObject;
    [SerializeField] RectTransform spawnAreaRect;
    [SerializeField] GameObject bonusTextPrefab;

    [Header("Reward Settings:")]
    [SerializeField] float bonusDivider = 25f;
    [SerializeField] float minBonusValue = 10f;

    [Header("Spawn Settings:")]
    [SerializeField] float minSpawnTime = 30f;
    [SerializeField] float maxSpawnTime = 120f;
    [SerializeField] float visibleDuration = 10f;

    [Header("Juice Animations:")]
    [SerializeField] float entryDuration = 0.4f;
    [SerializeField] float pulseSpeed = 6f;
    [SerializeField] float pulseAmount = 0.12f;
    [SerializeField] float rotateSpeed = 4f;
    [SerializeField] float maxRotateAngle = 10f;

    [Header("Collect Particle Animation:")]
    [SerializeField] float collectAnimationDuration = 0.5f;

    RectTransform buttonRect;
    Image buttonImage;
    Coroutine shrinkCoroutine;
    bool isCollectedAndAnimating = false;

    void Awake()
    {
        if (bonusButtonObject != null)
        {
            buttonRect = bonusButtonObject.GetComponent<RectTransform>();
            buttonImage = bonusButtonObject.GetComponent<Image>();
        }
    }

    void Start()
    {
        if (bonusButtonObject != null)
            bonusButtonObject.SetActive(false);

        StartCoroutine(BonusSpawnRoutine());
    }

    IEnumerator BonusSpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minSpawnTime, maxSpawnTime));

            while (isCollectedAndAnimating)
            {
                yield return null;
            }

            SpawnBonus();

            if (shrinkCoroutine != null) StopCoroutine(shrinkCoroutine);
            shrinkCoroutine = StartCoroutine(ShrinkButtonRoutine());

            yield return new WaitForSeconds(visibleDuration);

            if (bonusButtonObject != null && !isCollectedAndAnimating)
                bonusButtonObject.SetActive(false);
        }
    }

    IEnumerator ShrinkButtonRoutine()
    {
        float timer = 0;

        while (timer < visibleDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / visibleDuration;

            if (buttonRect != null && !isCollectedAndAnimating)
            {
                float currentScale = 1f;

                if (timer < entryDuration)
                {
                    float entryProgress = timer / entryDuration;
                    currentScale = Mathf.Sin(entryProgress * Mathf.PI * 0.5f) * 1.05f;
                }
                else
                {
                    float pulseFactor = Mathf.Sin((timer - entryDuration) * pulseSpeed);
                    currentScale = 1f + (pulseFactor * pulseAmount);

                    if (progress > 0.8f)
                    {
                        float shrinkProgress = (progress - 0.8f) / 0.2f;
                        currentScale = Mathf.Lerp(currentScale, 0f, shrinkProgress);
                    }
                }

                buttonRect.localScale = new Vector3(currentScale, currentScale, 1f);

                float rotationZ = Mathf.Cos(timer * rotateSpeed) * maxRotateAngle;
                buttonRect.localRotation = Quaternion.Euler(0f, 0f, rotationZ);
            }

            yield return null;
        }

        if (buttonRect != null && !isCollectedAndAnimating)
        {
            buttonRect.localScale = Vector3.zero;
            buttonRect.localRotation = Quaternion.identity;
        }
    }

    void SpawnBonus()
    {
        if (bonusButtonObject == null || spawnAreaRect == null || buttonRect == null) return;

        isCollectedAndAnimating = false;

        buttonRect.SetParent(spawnAreaRect, false);
        bonusButtonObject.transform.SetAsLastSibling();

        buttonRect.localScale = Vector3.zero;
        buttonRect.localRotation = Quaternion.identity;

        buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
        buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
        buttonRect.pivot = new Vector2(0.5f, 0.5f);

        float widthLimit = (spawnAreaRect.rect.width - 60f) / 2f;
        float heightLimit = (spawnAreaRect.rect.height - 60f) / 2f;

        float x = UnityEngine.Random.Range(-widthLimit, widthLimit);
        float y = UnityEngine.Random.Range(-heightLimit, heightLimit);

        buttonRect.anchoredPosition = new Vector2(x, y);
        bonusButtonObject.SetActive(true);
    }

    public void OnBonusClicked()
    {
        if (data == null || !bonusButtonObject.activeSelf || isCollectedAndAnimating) return;

        isCollectedAndAnimating = true;
        if (shrinkCoroutine != null) StopCoroutine(shrinkCoroutine);

        double divider = bonusDivider > 0 ? (double)bonusDivider : 1.0;
        double bonusAmount = Math.Max((double)minBonusValue, data.pointsCounterFloat / divider);

        System_Economy.Instance.AddPoints(bonusAmount);
        data.luckyBonus++;

        SpawnBonusText(NumberFormatter.FormatWithDots(bonusAmount));

        PointsDisplay display = UnityEngine.Object.FindAnyObjectByType<PointsDisplay>();
        if (display != null)
            display.PulseLuckyBonus();

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("Lucky Bonus");

        if (System_Leveling.Instance != null)
            System_Leveling.Instance.FlashGoldenDropColor();

        StartCoroutine(CollectParticleRoutine());
    }

    IEnumerator CollectParticleRoutine()
    {
        if (bonusButtonObject == null || buttonRect == null || buttonImage == null) yield break;

        GameObject particleObj = new GameObject("LuckyBonus_CollectFX", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        particleObj.transform.SetParent(bonusButtonObject.transform.parent, false);

        RectTransform partRect = particleObj.GetComponent<RectTransform>();
        Image partImg = particleObj.GetComponent<Image>();

        partRect.anchoredPosition = buttonRect.anchoredPosition;
        partRect.sizeDelta = buttonRect.sizeDelta;
        partRect.localScale = buttonRect.localScale;
        partRect.localRotation = buttonRect.localRotation;
        partImg.sprite = buttonImage.sprite;
        partImg.color = buttonImage.color;
        partImg.material = buttonImage.material;

        bonusButtonObject.SetActive(false);

        float elapsed = 0f;
        Vector3 startScale = partRect.localScale;
        Vector3 targetScale = startScale * 1.5f;
        Color startColor = partImg.color;

        while (elapsed < collectAnimationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / collectAnimationDuration;

            partRect.localScale = Vector3.Lerp(startScale, targetScale, t);
            partRect.localRotation *= Quaternion.Euler(0f, 0f, 360f * Time.deltaTime);
            partImg.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(1f, 0f, t));

            yield return null;
        }
        Destroy(particleObj);
        isCollectedAndAnimating = false;
    }

    void SpawnBonusText(string amountText)
    {
        if (bonusTextPrefab == null || bonusButtonObject == null || spawnAreaRect == null) return;

        GameObject textObj = Instantiate(bonusTextPrefab, spawnAreaRect);

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        if (textRect != null && buttonRect != null)
        {
            textRect.anchoredPosition = buttonRect.anchoredPosition;
        }

        FloatingBonusText floatingScript = textObj.GetComponent<FloatingBonusText>();
        if (floatingScript != null)
        {
            floatingScript.Initialize(amountText);
        }
        else
        {
            TextMeshProUGUI textComp = textObj.GetComponentInChildren<TextMeshProUGUI>();
            if (textComp != null) textComp.text = $"+{amountText}";
            Destroy(textObj, 2f);
        }
    }

    public bool IsBonusVisible()
    {
        return bonusButtonObject != null && bonusButtonObject.activeSelf;
    }
}