using System.Text;

namespace TitularRoyalty
{
    /// <summary>
    /// Provides methods to convert between integers and Roman numerals.
    /// </summary>
    [PublicAPI]
    public static class RomanNumerals
    {
        
        // Value of each roman numeral
        private static readonly Dictionary<char, int> RomanNumeralValues = new()
        {
            { 'I', 1 },
            { 'V', 5 },
            { 'X', 10 },
            { 'L', 50 },
            { 'C', 100 },
            { 'D', 500 },
            { 'M', 1000 }
        };

        // Dictionary to map integer values to their Roman numeral strings.
        private static readonly Dictionary<int, string> IntToRoman = new()
        {
            { 1000, "M" },
            { 900, "CM" },
            { 500, "D" },
            { 400, "CD" },
            { 100, "C" },
            { 90, "XC" },
            { 50, "L" },
            { 40, "XL" },
            { 10, "X" },
            { 9, "IX" },
            { 5, "V" },
            { 4, "IV" },
            { 1, "I" }
        };

        /// <summary>
        /// Converts an integer to its Roman numeral representation.
        /// </summary>
        /// <param name="number">The integer to convert.</param>
        /// <returns>The Roman numeral representation of the integer.</returns>
        public static string FromInt(int number)
        {
            var result = new StringBuilder();
            foreach (var (romanValue, romanChar) in IntToRoman)
            {
                while (number >= romanValue)
                {
                    result.Append(romanChar);
                    number -= romanValue;
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// Converts a Roman numeral string to its integer representation.
        /// </summary>
        /// <param name="roman">The Roman numeral string to convert.</param>
        /// <returns>The integer representation of the Roman numeral.</returns>
        public static int ToInt(string roman)
        {
            int total = 0;
            int previousValue = 0;

            foreach (var ch in roman)
            {
                int currentValue = RomanNumeralValues[ch];
                total += currentValue;
                
                // If the current value is greater than the previous value, we need to correct the total
                // by subtracting twice the previous value (once to undo the addition, and once to apply the correct subtraction)
                if (currentValue > previousValue)
                {
                    total -= 2 * previousValue;
                }

                previousValue = currentValue;
            }

            return total;
        }
    }
}
