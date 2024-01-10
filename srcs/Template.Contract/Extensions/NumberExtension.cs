using System.Globalization;

namespace Template.Contract.Extensions;

public static class NumberExtension
{
    public static double FormatN2(this double n)
    {
        string formattedString = n.ToString("N2");

        // Remove the thousands separator (comma) from the formatted string
        formattedString = formattedString.Replace(",", "");

        double formattedDouble = double.Parse(formattedString, CultureInfo.InvariantCulture);
        return formattedDouble;
    }
    public static double FormatF1(this double n)
    {
        string formattedString = n.ToString("F1");

        // Remove the thousands separator (comma) from the formatted string
        formattedString = formattedString.Replace(",", "");

        double formattedDouble = double.Parse(formattedString, CultureInfo.InvariantCulture);
        return formattedDouble;
    }

    public static string FormatStringN2(this double n)
    {
        string formattedString = n.ToString("N2");
        // Remove the thousands separator (comma) from the formatted string
        formattedString = formattedString.Replace(",", "");
        return formattedString;
    }
    public static double GetTopRankRevenueCompanySize(this double inputValue)
    {
        return inputValue switch
        {
            <= 20000000000 => 20000000000,
            <= 100000000000 => 100000000000,
            <= 500000000000 => 500000000000,
            _ => inputValue <= 1000000000000 ? 1000000000000 : 0
        };
    }
}
