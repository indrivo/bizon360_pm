using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Gear.Common.Extensions.StringExtensions
{
    public static class StringExtensions
    {
        public static string ToInitials(this string str)
        {
            Regex extractInitials =
                new Regex(@"(?i)(?:^|\s|-)+([^\s-])[^\s-]*(?:(?:\s+)(?:the\s+)?(?:jr|sr|II|2nd|III|3rd|IV|4th)\.?$)?");
            return extractInitials.Replace(str, "$1").ToUpper();

        }

        public static List<string> FindEmails(this string text)
        {
            const string MatchEmailPattern =
                @"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})";
            Regex rx = new Regex(MatchEmailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            // Find matches.
            MatchCollection matches = rx.Matches(text);

            var emails = new List<string>();

            foreach (Match match in matches)
            {
                emails.Add(match.ToString());
            }

            return emails;
        }

        public static string GetInitialsFromEmail(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
                
            str = str.Split('@')[0];
            str = str.Replace('.', ' ');
            return str.ToInitials();
        }

        public static string DisplayEmploymentTypeName(this string str)
        {
            if (str.Contains("PartTime4H")) return "4.0 h (part-time)";

            if (str.Contains("PartTime6H")) return "6.0 h (part-time)";

            return "8.0 h (full-time)";
        }

        public static string GetUntilOrEmpty(this string text, string stopAt = "_")
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;

            var charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

            return charLocation > 0 ? text.Substring(0, charLocation) : string.Empty;
        }

        public static string GetAfterSymbol(this string value, string startFrom)
        {
            var posA = value.LastIndexOf(startFrom, StringComparison.Ordinal);

            if (posA == -1) return "";

            var adjustedPosA = posA + startFrom.Length;

            return adjustedPosA >= value.Length ? "" : value.Substring(adjustedPosA);
        }
        
        public static string TimeAgo(this System.DateTime dateTime)
        {
            string result;

            var timeSpan = System.DateTime.Now.Subtract(dateTime);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = $"{timeSpan.Seconds} seconds ago";
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ?
                    $"{timeSpan.Minutes} minutes ago" :
                    "a minute ago";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ?
                    $"{timeSpan.Hours} hours ago" :
                    "an hour ago";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1 ?
                    $"{timeSpan.Days} days ago" :
                    "yesterday";
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ?
                    $"{timeSpan.Days / 30} months ago" :
                    "a month ago";
            }
            else
            {
                result = timeSpan.Days > 365 ?
                    $"{timeSpan.Days / 365} years ago" :
                    "a year ago";
            }

            return result;
        }

        public static int GetCustomId(this string str)
        {
            if (str.Length < 6)
                return -1;

            str = str.Substring(1, str.Length - 1);

            if (char.IsDigit(str[0]))
                str = str.TrimStart('0');
            else
                return -1;

            if (int.TryParse(str, out int result))
                return result;
            
            return -1;
        }

        public static string ReplaceSpecialCharacters(this string str, char character)
        {
            return str.Replace('\\', character)
                .Replace('/', character)
                .Replace(':', character)
                .Replace('*', character)
                .Replace('?', character)
                .Replace('<', character)
                .Replace('>', character)
                .Replace('"', character);
        }
    }
}
