using System.Globalization;

namespace ParkingSystem.Utils
{
    public static class StringHelper
    {
        public static string ToTitleCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var cultureInfo = new CultureInfo("pt-BR");
            return cultureInfo.TextInfo.ToTitleCase(input.Trim().ToLower());
        }

        public static string NormalizeUpper(string input)
        {
            return string.IsNullOrWhiteSpace(input)
                ? string.Empty
                : input.Trim().ToUpper();
        }
    }
}