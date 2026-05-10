using UnityEngine;

public class Mastery : MonoBehaviour
{
    public static Mastery Instance;

    [SerializeField] System_Data data;

    public enum MasteryType { Click, Combo, Critical, AwayIncome }

    void Awake() { Instance = this; }

    public void AddMasteryXP(MasteryType type, float amount)
    {
        switch (type)
        {
            case MasteryType.Click:
                data.clickMasteryXP += amount;
                CheckLevelUp(ref data.clickMasteryLvl, ref data.clickMasteryXP, 100);
                break;

            case MasteryType.Combo:
                data.comboMasteryXP += amount;
                CheckLevelUp(ref data.comboMasteryLvl, ref data.comboMasteryXP, 150);
                break;

            case MasteryType.Critical:
                data.critMasteryXP += amount;
                CheckLevelUp(ref data.critMasteryLvl, ref data.critMasteryXP, 200);
                break;

            case MasteryType.AwayIncome:
                data.awayMasteryXP += amount;
                CheckLevelUp(ref data.awayMasteryLvl, ref data.awayMasteryXP, 500);
                break;
        }
    }

    void CheckLevelUp(ref int level, ref float currentXP, float baseNeeded)
    {
        int maxLevel = 200;

        if (level >= maxLevel)
        {
            currentXP = 0;
            return;
        }

        float needed = baseNeeded * Mathf.Pow(1.2f, level);

        if (currentXP >= needed)
        {
            currentXP -= needed;
            level++;

            if (level >= maxLevel)
            {
                level = maxLevel;
                currentXP = 0;
            }
        }
    }

    public float GetMasteryBonus(MasteryType type)
    {
        switch (type)
        {
            case MasteryType.Click:
                return data.clickMasteryLvl * 0.005f;

            case MasteryType.Critical:
                return data.critMasteryLvl * 0.01f;

            case MasteryType.Combo:
                return data.comboMasteryLvl * 0.01f;

            case MasteryType.AwayIncome:
                return data.awayMasteryLvl * 0.01f;

            default: return 0f;
        }
    }
}
