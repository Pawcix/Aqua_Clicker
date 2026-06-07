using UnityEngine;

public class Mastery_AwayIncome : MonoBehaviour
{
    public void RegisterAwayIncomeXP(float amount)
    {
        if (Mastery.Instance != null)
        {
            Mastery.Instance.AddMasteryXP(Mastery.MasteryType.AwayIncome, amount);
        }
    }
}