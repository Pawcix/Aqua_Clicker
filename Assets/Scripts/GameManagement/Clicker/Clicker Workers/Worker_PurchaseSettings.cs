using UnityEngine;

public class Worker_PurchaseSettings : MonoBehaviour
{
    public enum PurchaseMode { x1, x5, x10, Max }
    public static PurchaseMode CurrentMode = PurchaseMode.x1;

    public static void CycleMode()
    {
        if (CurrentMode == PurchaseMode.x1) CurrentMode = PurchaseMode.x5;
        else if (CurrentMode == PurchaseMode.x5) CurrentMode = PurchaseMode.x10;
        else if (CurrentMode == PurchaseMode.x10) CurrentMode = PurchaseMode.Max;
        else CurrentMode = PurchaseMode.x1;
    }
}
