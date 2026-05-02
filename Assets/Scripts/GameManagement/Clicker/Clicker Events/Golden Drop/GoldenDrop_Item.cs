using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldenDrop_Item : MonoBehaviour
{
    [Header("Settings:")]
    [SerializeField] float fallSpeed = 300f;
    [SerializeField] GameObject bonusTextPrefab;

    System_Data data;
    RectTransform rectTransform;

    float existenceTime = 0f;
    public float ExistenceTime => existenceTime;
    bool wasCollected = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        data = Object.FindFirstObjectByType<System_Data>();
        Button btn = GetComponent<Button>();
        if (btn != null) btn.onClick.AddListener(OnGoldenDropClicked);

        Destroy(gameObject, 10f);
    }

    void Update()
    {
        if (wasCollected) return;

        existenceTime += Time.deltaTime;

        if (rectTransform != null)
            rectTransform.anchoredPosition += Vector2.down * fallSpeed * Time.deltaTime;
    }

    public void OnGoldenDropClicked()
    {
        if (data == null || wasCollected) return;
        wasCollected = true;

        double currentPoints = data.pointsCounterFloat;
        double bonus = currentPoints * 0.10;

        if (bonus < 100.0) bonus = 100.0;

        data.pointsCounterFloat += bonus;
        data.goldenDrops++;

        if (PointsDisplay.Instance != null)
        {
            PointsDisplay.Instance.Pulse();
        }
        else
        {
            PointsDisplay display = Object.FindFirstObjectByType<PointsDisplay>();
            if (display != null) display.Pulse();
        }

        Clicker_Prefabs prefabsManager = Object.FindFirstObjectByType<Clicker_Prefabs>();
        if (prefabsManager != null)
        {
            int displayPPS = (int)System.Math.Floor(data.pointsPerSecond);
            prefabsManager.UpdateAllPrefabs(data.pointsCounterFloat, displayPPS);
        }

        SpawnBonusText(NumberFormatter.FormatWithDots(bonus));

        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("GoldenCollect");

        Destroy(gameObject);
    }

    public void SetSpeed(float newSpeed)
    {
        fallSpeed = newSpeed;
    }

    void SpawnBonusText(string amountText)
    {
        if (bonusTextPrefab == null) return;

        GameObject textObj = Instantiate(bonusTextPrefab, transform.position, Quaternion.identity, transform.parent);
        TextMeshProUGUI textComp = textObj.GetComponentInChildren<TextMeshProUGUI>();

        if (textComp != null) textComp.text = $"+{amountText}";

        Destroy(textObj, 2f);
    }
}
