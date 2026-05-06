using UnityEngine;

public class System_Notification : MonoBehaviour
{
    public static System_Notification Instance;
    public GameObject badgeObject;
    [SerializeField] System_Data data;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        CheckGlobalNotification();
    }

    public void CheckGlobalNotification()
    {
        if (data == null || badgeObject == null) return;

        bool hasAnyNewSkin = false;
        foreach (int id in data.unlockedSkinIDs)
        {
            if (id == 0) continue;

            if (!data.seenSkinIDs.Contains(id))
            {
                hasAnyNewSkin = true;
                break;
            }
        }
        badgeObject.SetActive(hasAnyNewSkin);
    }

    public void SetAlert(bool state)
    {
        if (badgeObject != null) badgeObject.SetActive(state);
    }
}
