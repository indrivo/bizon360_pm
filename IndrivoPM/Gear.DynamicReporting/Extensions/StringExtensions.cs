using System.Text;

namespace Gear.DynamicReporting.Abstractions.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// This method returns the next combination of the string. Thus for the string "C" the next combination will
        /// be "D", for "BG" - "BH" and for "ZZ" the result will be "AAA". Only for uppercase strings of english alphabet!!!
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Next(this string value)
        {
            var result = new StringBuilder(value);

            var isConditionSatisfied = false;
            for (var index = value.Length; index-- > 0 && !isConditionSatisfied; )
            {
                if ((result[index] == 'Z')) continue;
                isConditionSatisfied = true;
                result[index]++;
                for (var right = index + 1; right < value.Length; right++)
                {
                    result[right] = 'A';
                }
            }

            if (isConditionSatisfied)
                return result.ToString();

            var alternative = new StringBuilder();
            for (var i = 0; i <= value.Length; i++)
            {
                alternative.Append('A');
            }
            return alternative.ToString();
        }
    }
}
