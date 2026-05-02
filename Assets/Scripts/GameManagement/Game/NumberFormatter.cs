using UnityEngine;
using System.Globalization;

public class NumberFormatter : MonoBehaviour
{
    public static string FormatWithDots(double value)
    {
        NumberFormatInfo nfi = new NumberFormatInfo();
        nfi.NumberGroupSeparator = ".";
        nfi.NumberDecimalSeparator = ",";
        nfi.NumberGroupSizes = new int[] { 3 };

        return System.Math.Floor(value).ToString("N0", nfi);
    }
}