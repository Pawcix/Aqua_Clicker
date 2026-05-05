// using UnityEngine;

// public enum NotificationType { Skin, Achievement, ShopItem, GeneralWardrobe }

// public class System_Notification : MonoBehaviour
// {
//     public static System_Notification Instance;

//     [SerializeField] System_Data data; 

//     void Awake()
//     {
//         if (Instance == null) Instance = this;
//     }

//     public bool ShouldShowNotification(NotificationType type, int id)
//     {
//         if (data == null) return false;

//         switch (type)
//         {
//             case NotificationType.Skin:
//                 // Sprawdza konkretny skin po ID
//                 return data.unlockedSkinIDs.Contains(id) && !data.seenSkinIDs.Contains(id);

//             case NotificationType.GeneralWardrobe:
//                 // Sprawdza, czy w CAŁEJ garderobie jest jakikolwiek nieobejrzany skin
//                 // Pętla przechodzi przez wszystkie odblokowane i sprawdza czy są w "seen"
//                 foreach (int unlockedID in data.unlockedSkinIDs)
//                 {
//                     if (!data.seenSkinIDs.Contains(unlockedID)) return true;
//                 }
//                 return false;

//             case NotificationType.Achievement:
//                 return !data.unlockedAchievementIDs.Contains(id.ToString());

//             default:
//                 return false;
//         }
//     }

//     // Metoda, która odświeży wszystkie kropki na scenie
//     public void RefreshAllBadges()
//     {
//         System_NotificationBadge[] allBadges = Object.FindObjectsByType<System_NotificationBadge>(FindObjectsSortMode.None);
//         foreach (var badge in allBadges)
//         {
//             badge.Refresh();
//         }
//     }
// }