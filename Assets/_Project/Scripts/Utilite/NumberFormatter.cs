using System;
using System.Globalization;

public static class NumberFormatter
{
    private static readonly string[] Suffixes =
    {
        "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc"
    };

    public static string NumberAsString(float value)
    {
        if (value < 1000)
            return value.ToString("0", CultureInfo.InvariantCulture);

        int suffix = 0;

        while (value >= 1000f && suffix < Suffixes.Length - 1)
        {
            value /= 1000f;
            suffix++;
        }

        if (MathF.Round(value, 1) >= 1000f && suffix < Suffixes.Length - 1)
        {
            value /= 1000f;
            suffix++;
        }

        return value.ToString(value >= 100 ? "0" : "0.#", CultureInfo.InvariantCulture)
               + Suffixes[suffix];
    }
}