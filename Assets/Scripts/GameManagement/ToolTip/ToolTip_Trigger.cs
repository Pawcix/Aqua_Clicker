using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea(3, 10)]
    [SerializeField] string content;
    [SerializeField] float delay = 2.0f;

    Coroutine delayCoroutine;

    public void OnPointerEnter(PointerEventData eventData)
    {
        delayCoroutine = StartCoroutine(WaitAndShow());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (delayCoroutine != null) StopCoroutine(delayCoroutine);
        ToolTip.Instance.HideTooltip();
    }

    IEnumerator WaitAndShow()
    {
        yield return new WaitForSeconds(delay);
        ToolTip.Instance.ShowTooltip(content, transform.position);
    }
}