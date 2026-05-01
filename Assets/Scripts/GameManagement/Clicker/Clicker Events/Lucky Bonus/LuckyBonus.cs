using TMPro;
using UnityEngine;
using System.Collections;

public class LuckyBonus : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] System_Data data;
    [SerializeField] GameObject bonusButtonObject;
    [SerializeField] RectTransform canvasRect;
    [SerializeField] GameObject bonusTextPrefab;

    [Header("Reward Settings:")]
    [SerializeField] float bonusDivider = 25f;
    [SerializeField] float minBonusValue = 10f;

    [Header("Spawn Settings:")]
    [SerializeField] float minSpawnTime = 30f;
    [SerializeField] float maxSpawnTime = 120f;
    [SerializeField] float visibleDuration = 10f;

    RectTransform buttonRect;
    Coroutine shrinkCoroutine;

    void Awake()
    {
        if (bonusButtonObject != null)
            buttonRect = bonusButtonObject.GetComponent<RectTransform>();
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
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));

            SpawnBonus();

            if (shrinkCoroutine != null) StopCoroutine(shrinkCoroutine);
            shrinkCoroutine = StartCoroutine(ShrinkButtonRoutine());

            yield return new WaitForSeconds(visibleDuration);
            bonusButtonObject.SetActive(false);
        }
    }

    IEnumerator ShrinkButtonRoutine()
    {
        float timer = 0;
        Vector3 initialScale = Vector3.one;

        while (timer < visibleDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / visibleDuration;

            if (buttonRect != null)
            {
                buttonRect.localScale = Vector3.Lerp(initialScale, Vector3.zero, progress);
            }

            yield return null;
        }

        buttonRect.localScale = Vector3.zero;
    }

    void SpawnBonus()
    {
        if (bonusButtonObject == null || canvasRect == null || buttonRect == null) return;

        buttonRect.localScale = Vector3.one;
        buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
        buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
        buttonRect.pivot = new Vector2(0.5f, 0.5f);

        float widthLimit = (canvasRect.rect.width - 100) / 2f;
        float heightLimit = (canvasRect.rect.height - 100) / 2f;

        float x = Random.Range(-widthLimit, widthLimit);
        float y = Random.Range(-heightLimit, heightLimit);

        buttonRect.anchoredPosition = new Vector2(x, y);
        bonusButtonObject.SetActive(true);
    }

    public void OnBonusClicked()
    {
        if (data == null || !bonusButtonObject.activeSelf) return;
        if (shrinkCoroutine != null) StopCoroutine(shrinkCoroutine);

        float divider = bonusDivider > 0 ? bonusDivider : 1;
        float bonusAmount = Mathf.Max(minBonusValue, data.pointsCounterFloat / divider);
        int finalAmount = Mathf.RoundToInt(bonusAmount);

        data.pointsCounterFloat += bonusAmount;
        data.luckyBonus++;

        SpawnBonusText(NumberFormatter.FormatWithDots(finalAmount));

        if (Object.FindFirstObjectByType<PointsDisplay>() != null)
            Object.FindFirstObjectByType<PointsDisplay>().Pulse();

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("Buy Sound");

        bonusButtonObject.SetActive(false);
    }

    void SpawnBonusText(string amountText)
    {
        if (bonusTextPrefab == null || bonusButtonObject == null) return;
        GameObject textObj = Instantiate(bonusTextPrefab, bonusButtonObject.transform.position, Quaternion.identity, bonusButtonObject.transform.parent);
        TextMeshProUGUI textComp = textObj.GetComponentInChildren<TextMeshProUGUI>();
        if (textComp != null) textComp.text = $"+{amountText}";
        Destroy(textObj, 2f);
    }
}
