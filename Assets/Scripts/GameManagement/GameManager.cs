using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class ClickerManager : MonoBehaviour
{
    public static UnityEvent<int, int> OnItemBought = new UnityEvent<int, int>();

    [Header("General")]
    [SerializeField] ClickerUI clickerUI;
    [SerializeField] Button clickerButton;
    [SerializeField] TextMeshProUGUI counterPointsPerSecond;
    [SerializeField] TextMeshProUGUI counterAfterClick;

    [Header("Shop")]
    [SerializeField] ShopUI shopUI;
    [SerializeField] Button OpenButtonShop;
    [SerializeField] Button CloseButtonShop;
    [SerializeField] List<Worker> Workers;

    [Header("Stats")]
    [SerializeField] StatsUI statsUI;
    [SerializeField] Button OpenButtonStats;
    [SerializeField] Button CloseButtonStats;

    [Header("Settings")]
    [SerializeField] SettingsUI settingsUI;
    [SerializeField] Button OpenButtonSettings;
    [SerializeField] Button CloseButtonSettings;

    [Header("Animation")]
    [SerializeField] AnimationManager animationUI;

    [Header("DoubleClick Button status")]
    [SerializeField] Image autoClickButtonImage;
    [SerializeField] Sprite neutralIcon;
    [SerializeField] Sprite activeIcon;
    [SerializeField] Button doublePointsButton;
    bool isDoublePointsActive = false;
    Coroutine doublePointsCoroutine;

    int pointsCounter = 0;
    int pointsPerSecond = 0;
    int clicksNumber = 0;
    int amountPurchase = 0;

    int allPoints = 0;
    int colectPoints = 0;
    int colectPointsPerSecond = 0;
    int colectPointsPerClick = 0;

    string[] waterWords = { "Plunk", "Bloop", "Splish", "Blop", "Plip-plop" };

    public ShopWorkerUI shopWorkerUI;

    void Start()
    {
        clickerUI.UpdateUI(pointsCounter);

        clickerButton.onClick.AddListener(AddPoints);
        clickerButton.onClick.AddListener(AmountClicks);
        clickerButton.onClick.AddListener(CounterAfterClickOnButton);
        clickerButton.onClick.AddListener(Scale);

        doublePointsButton.onClick.AddListener(ToggleDoublePoints);

        OpenCloseElementsUI();

        foreach (Worker worker in Workers)
        {
            shopUI.AddWorker(worker);
        }

        OnItemBought.AddListener(BuyItem);

        InvokeRepeating(nameof(AddPointPerSecond), 0f, 1f);
        InvokeRepeating(nameof(CollectAllPointsInGame), 0f, 1f);
    }

    void OpenCloseElementsUI()
    {
        shopUI.CloseShop();
        OpenButtonShop.onClick.AddListener(shopUI.OpenShop);
        CloseButtonShop.onClick.AddListener(shopUI.CloseShop);

        statsUI.CloseStats();
        OpenButtonStats.onClick.AddListener(statsUI.OpenStats);
        CloseButtonStats.onClick.AddListener(statsUI.CloseStats);

        settingsUI.CloseSettings();
        OpenButtonSettings.onClick.AddListener(settingsUI.OpenSettings);
        CloseButtonSettings.onClick.AddListener(settingsUI.CloseSettings);
    }

    void ColectPointsPerSecond()
    {
        colectPointsPerSecond += pointsPerSecond;
        colectPoints = colectPointsPerSecond;

        clickerUI.ColectPointsPerSecond(colectPoints);
    }

    void CollectAllPointsInGame()
    {
        colectPointsPerClick = clicksNumber;
        allPoints = colectPointsPerClick;

        ColectPointsPerSecond();
        clickerUI.ColectAllPoints(allPoints + colectPoints);
    }

    void AddPointPerSecond()
    {
        if (pointsPerSecond > 0)
        {
            pointsCounter += pointsPerSecond;

            clickerUI.ClickPerSecondUpdateUI(pointsPerSecond);
            clickerUI.UpdateUI(pointsCounter);
        }
    }

    void UpdateShopUI(Worker worker)
    {
        if (shopWorkerUI != null)
        {
            shopWorkerUI.UpdateUI(worker);
        }
    }

    void BuyItem(int price, int power)
    {
        if (price <= pointsCounter)
        {
            amountPurchase++;
            pointsPerSecond += power;
            pointsCounter -= price;

            AudioManager.Instance.PlaySFX("Buy Sound");
            clickerUI.ClickPerSecondUpdateUI(pointsPerSecond);
            counterPointsPerSecond.text = $"Per Second\n" + pointsPerSecond.ToString() + " /s";
            clickerUI.AmountPurchasesUpdateUI(amountPurchase);
            clickerUI.UpdateUI(pointsCounter);
        }
    }

    void AddPoints()
    {
        pointsCounter += isDoublePointsActive ? 2 : 1;
        clickerUI.UpdateUI(pointsCounter);
        RandomLocationCounterAfterClick();
        StartCoroutine(animationUI.PulseAnimation());
        AudioManager.Instance.PlaySFX("Water Click");
    }

    void ToggleDoublePoints()
    {
        if (isDoublePointsActive)
        {
            autoClickButtonImage.sprite = neutralIcon;
            StopCoroutine(doublePointsCoroutine);
            isDoublePointsActive = false;
        }
        else
        {
            autoClickButtonImage.sprite = activeIcon;
            isDoublePointsActive = true;
            doublePointsCoroutine = StartCoroutine(DisableDoublePointsAfterDelay(10f));
        }
    }

    IEnumerator DisableDoublePointsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isDoublePointsActive = false;
    }

    void AmountClicks()
    {
        clicksNumber++;
        clickerUI.CounterClickUpdateUI(clicksNumber);
    }

    void RandomLocationCounterAfterClick()
    {
        counterAfterClick.raycastTarget = false;

        RectTransform rectTransform = counterAfterClick.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(UnityEngine.Random.Range(-80, 80), UnityEngine.Random.Range(-130, 130));
    }

    void Scale()
    {
        clickerButton.transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
        StartCoroutine(ResetScaleAfterDelay(0.1f));
    }

    IEnumerator ResetScaleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        clickerButton.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    void CounterAfterClickOnButton()
    {
        float rotationZ = UnityEngine.Random.Range(-30f, 30f);

        counterAfterClick.text = waterWords[UnityEngine.Random.Range(0, waterWords.Length)];
        counterAfterClick.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
        StartCoroutine(ResetTextAfterDelay(0.25f));
    }

    IEnumerator ResetTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        counterAfterClick.text = "";
        counterAfterClick.transform.rotation = Quaternion.identity;
    }
}
