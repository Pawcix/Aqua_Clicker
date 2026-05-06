using UnityEngine;

public class System_NotificationSkin : MonoBehaviour
{
    [SerializeField] private GameObject badgeObject;

    public void SetBadge(bool state)
    {
        if (badgeObject != null)
            badgeObject.SetActive(state);
    }
}
