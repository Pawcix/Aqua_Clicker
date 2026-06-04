using UnityEngine;
using UnityEngine.EventSystems;

public class UI_HoverOverlay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject hoverOverlayObject;

    void Awake()
    {
        if (hoverOverlayObject != null)
        {
            hoverOverlayObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverOverlayObject != null)
        {
            hoverOverlayObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverOverlayObject != null)
        {
            hoverOverlayObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        if (hoverOverlayObject != null)
        {
            hoverOverlayObject.SetActive(false);
        }
    }
}