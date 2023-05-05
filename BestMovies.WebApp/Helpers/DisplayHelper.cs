using System.Globalization;

namespace BestMovies.WebApp.Helpers;

public static class DisplayHelper
{
    public static string DisplayValue(decimal value, CultureInfo culture, int numberOfDecimals = 2)
    {
        return value is decimal.Zero
            ? value.ToString("0.#", culture)
            : Math.Round(value, numberOfDecimals, MidpointRounding.AwayFromZero).ToString($"N{numberOfDecimals}", culture);
    }
}