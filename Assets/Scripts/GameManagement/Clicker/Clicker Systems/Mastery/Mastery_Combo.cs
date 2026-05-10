using UnityEngine;

public class Mastery_Combo : MonoBehaviour
{
    public void RegisterComboXP(float amount = 2f)
    {
        if (Mastery.Instance != null)
        {
            Mastery.Instance.AddMasteryXP(Mastery.MasteryType.Combo, amount);
        }
    }
}
