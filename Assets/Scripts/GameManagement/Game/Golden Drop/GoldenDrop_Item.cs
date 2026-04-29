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

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        data = Object.FindFirstObjectByType<System_Data>();

        UnityEngine.UI.Button btn = GetComponent<UnityEngine.UI.Button>();
        if (btn != null) btn.onClick.AddListener(OnGoldenDropClicked);

        Destroy(gameObject, 10f);
    }

    void Update()
    {
        if (rectTransform != null)
            rectTransform.anchoredPosition += Vector2.down * fallSpeed * Time.deltaTime;
    }

    public void OnGoldenDropClicked()
    {
        if (data == null) return;

        int bonus = Mathf.RoundToInt(data.pointsCounter * 0.10f);
        if (bonus < 100) bonus = 100;

        data.pointsCounter += bonus;

        string formattedBonus = NumberFormatter.FormatWithDots(bonus);
        SpawnBonusText(formattedBonus);

        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("GoldenCollect");

        Destroy(gameObject);
    }

    void SpawnBonusText(string amountText)
    {
        if (bonusTextPrefab == null) return;

        GameObject textObj = Instantiate(bonusTextPrefab, transform.position, Quaternion.identity, transform.parent);
        TextMeshProUGUI textComp = textObj.GetComponentInChildren<TextMeshProUGUI>();

        if (textComp != null)
        {
            textComp.text = $"+{amountText}";
        }

        Destroy(textObj, 2f);
    }
}
