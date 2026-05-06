using UnityEngine;

public class System_WardrobeUnlockSkin : MonoBehaviour
{
    public static System_WardrobeUnlockSkin Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void GrantReward(Achievement achievement)
    {
        if (achievement == null || achievement.rewardSkinID == -1) return;

        if (System_Wardrobe.Instance != null)
        {
            System_Wardrobe.Instance.UnlockSkin(achievement.rewardSkinID);

            if (System_Notification.Instance != null)
            {
                System_Notification.Instance.SetAlert(true);
            }
        }
    }
}
