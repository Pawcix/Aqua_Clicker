using UnityEngine;
using System.Globalization;

public class NumberFormatter : MonoBehaviour
{
    public static string FormatWithDots(int number)
    {
        CultureInfo cultureInfo = new CultureInfo("pl-PL");
        cultureInfo.NumberFormat.NumberGroupSeparator = ".";
        return number.ToString("N0", cultureInfo);
    }
}
