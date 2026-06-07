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
                CheckLevelUp(ref data.clickMasteryLvl, ref data.clickMasteryXP, 100f, 1.10f);
                break;

            case MasteryType.Combo:
                data.comboMasteryXP += amount;
                CheckLevelUp(ref data.comboMasteryLvl, ref data.comboMasteryXP, 150f, 1.12f);
                break;

            case MasteryType.Critical:
                data.critMasteryXP += amount;
                CheckLevelUp(ref data.critMasteryLvl, ref data.critMasteryXP, 200f, 1.15f);
                break;

            case MasteryType.AwayIncome:
                data.awayMasteryXP += amount;
                CheckLevelUp(ref data.awayMasteryLvl, ref data.awayMasteryXP, 250f, 1.12f);
                break;
        }
    }

    void CheckLevelUp(ref int level, ref float currentXP, float baseNeeded, float multiplier)
    {
        int maxLevel = 200;

        if (level >= maxLevel)
        {
            currentXP = 0;
            return;
        }

        int loopSafety = 0;
        float needed = baseNeeded * Mathf.Pow(multiplier, level);

        while (currentXP >= needed && loopSafety < 100)
        {
            currentXP -= needed;
            level++;

            if (level >= maxLevel)
            {
                level = maxLevel;
                currentXP = 0;
                break;
            }

            needed = baseNeeded * Mathf.Pow(multiplier, level);
            loopSafety++;
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