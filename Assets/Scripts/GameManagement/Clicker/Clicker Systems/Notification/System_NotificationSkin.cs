// using UnityEngine;

// public class System_NotificationSkin : MonoBehaviour
// {
//     [SerializeField] NotificationType type;
//     [SerializeField] GameObject badgeVisual;
//     [SerializeField] int targetID; // Dla skina to ID skina, dla menu głównego może być -1

//     // Wywołujemy to przy otwieraniu okna lub odblokowaniu czegoś
//     public void Refresh()
//     {
//         if (System_Notification.Instance == null) return;

//         bool show = System_Notification.Instance.ShouldShowNotification(type, targetID);
//         if (badgeVisual != null) badgeVisual.SetActive(show);
//     }

//     // Opcjonalnie: ustawienie ID dynamicznie (np. przy spawnowaniu listy skinów)
//     public void Setup(NotificationType newType, int newID)
//     {
//         type = newType;
//         targetID = newID;
//         Refresh();
//     }
// }
