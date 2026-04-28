using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class System_NewsBaner : MonoBehaviour
{
    [Header("UI Elements:")]
    [SerializeField] GameObject bannerObject;
    [SerializeField] RectTransform newsTextRect;
    [SerializeField] TextMeshProUGUI newsText;

    [Header("Settings:")]
    [SerializeField] float scrollSpeed = 200f;
    [SerializeField] float pauseBetweenNews = 3f;

    [Header("Content:")]
    [TextArea(3, 10)]
    [SerializeField] List<string> newsMessages;

    Coroutine currentRoutine;
    bool isClickForced = false;

    void Start()
    {
        if (newsMessages.Count > 0)
            currentRoutine = StartCoroutine(Marquee());
    }

    public void OnBannerClicked()
    {
        isClickForced = true;

        StopAllCoroutines();
        newsTextRect.localPosition = new Vector3(9999, 0, 0);
        bannerObject.SetActive(false);

        isClickForced = false;
        currentRoutine = StartCoroutine(Marquee());
    }

    IEnumerator Marquee()
    {
        while (true)
        {
            bannerObject.SetActive(false);
            newsTextRect.localPosition = new Vector3(9999, 0, 0);
            yield return new WaitForSeconds(pauseBetweenNews);

            bannerObject.SetActive(true);
            yield return new WaitForSeconds(1f);

            string randomNews = newsMessages[Random.Range(0, newsMessages.Count)];
            newsText.text = randomNews;

            yield return StartCoroutine(ScrollTextRoutine());
            yield return new WaitForSeconds(1f);
            bannerObject.SetActive(false);
        }
    }

    IEnumerator ScrollTextRoutine()
    {
        RectTransform bannerRect = bannerObject.GetComponent<RectTransform>();
        float bannerWidth = bannerRect.rect.width;
        float realTextWidth = newsText.preferredWidth;
        float startX = (bannerWidth / 2f) + (realTextWidth / 2f);
        float endX = -(bannerWidth / 2f) - (realTextWidth / 2f);

        newsTextRect.localPosition = new Vector3(startX, 0, 0);

        while (newsTextRect.localPosition.x > endX)
        {
            if (isClickForced) break;

            float newX = newsTextRect.localPosition.x - (scrollSpeed * Time.deltaTime);
            newsTextRect.localPosition = new Vector3(newX, 0, 0);

            yield return null;
        }

        isClickForced = false;
    }
}
