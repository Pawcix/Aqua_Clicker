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

        float bonus = data.pointsCounterFloat * 0.10f;
        if (bonus < 100f) bonus = 100f;

        data.pointsCounterFloat += bonus;
        data.goldenDrops++;

        Clicker_Prefabs prefabsManager = Object.FindFirstObjectByType<Clicker_Prefabs>();
        if (prefabsManager != null)
        {
            prefabsManager.UpdateAllPrefabs((int)data.pointsCounterFloat, data.pointsPerSecond);
        }

        SpawnBonusText(NumberFormatter.FormatWithDots(Mathf.RoundToInt(bonus)));

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
