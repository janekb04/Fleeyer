using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

public static class Utils
{
    static NumberFormatInfo nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();

    static Utils()
    {
        nfi.NumberGroupSeparator = "'";
    }

    public static string IntToString(int val)
    {
        return val.ToString("#,0", nfi);
    }
}
