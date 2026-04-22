using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTip_Trigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]
    public string content;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolTip.Instance.ShowTooltip(content);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTip.Instance.HideTooltip();
    }
}
