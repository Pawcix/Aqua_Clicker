using UnityEngine;

public class Credits : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] RectTransform creditsBox;

    [Header("Movement Settings:")]
    [SerializeField] float scrollSpeed = 40f;
    [SerializeField] float startYOffset = -1000f;
    [SerializeField] float endYPosition = 1200f;

    Vector2 initialPosition;
    bool isScrolling = false;

    void Awake()
    {
        if (creditsBox != null)
        {
            initialPosition = creditsBox.anchoredPosition;
        }
    }

    void Update()
    {
        if (creditsBox == null || !isScrolling) return;

        creditsBox.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (creditsBox.anchoredPosition.y >= endYPosition)
        {
            ResetToStart();
        }
    }

    public void ResetAndStartScroll()
    {
        ResetToStart();
        isScrolling = true;
    }

    public void StopScroll()
    {
        isScrolling = false;
    }

    void ResetToStart()
    {
        if (creditsBox != null)
        {
            creditsBox.anchoredPosition = new Vector2(initialPosition.x, startYOffset);
        }
    }
}