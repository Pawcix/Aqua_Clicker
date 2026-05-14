using UnityEngine;

public class Event_Manager : MonoBehaviour
{
    public static Event_Manager Instance;
    [SerializeField] Transform bannerContainer;

    void Awake()
    {
        Instance = this;
    }

    public GameObject AddEventIcon(GameObject prefab)
    {
        if (prefab == null || bannerContainer == null) return null;
        GameObject newIcon = Instantiate(prefab, bannerContainer);
        newIcon.SetActive(true);
        return newIcon;
    }

    public void RemoveEventIcon(GameObject iconInstance)
    {
        if (iconInstance != null)
        {
            Destroy(iconInstance);
        }
    }
}