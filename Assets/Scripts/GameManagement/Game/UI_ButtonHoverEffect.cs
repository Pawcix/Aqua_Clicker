using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ButtonScaleOnly : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Scale Settings:")]
    [SerializeField] float hoverScale = 1.25f;
    [SerializeField] float scaleSpeed = 25f;

    Vector3 initialScale;
    Vector3 targetScale;
    Vector3 currentTarget;

    void Awake()
    {
        initialScale = transform.localScale;
        targetScale = initialScale * hoverScale;
        currentTarget = initialScale;
    }

    void OnDisable()
    {
        transform.localScale = initialScale;
        currentTarget = initialScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, currentTarget, Time.deltaTime * scaleSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        currentTarget = targetScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        currentTarget = initialScale;
    }
}