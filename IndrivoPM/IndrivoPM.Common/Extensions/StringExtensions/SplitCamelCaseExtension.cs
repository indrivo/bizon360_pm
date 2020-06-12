﻿using System.Text.RegularExpressions;

namespace Gear.Common.Extensions.StringExtensions
{
    public static class SplitCamelCaseExtension
    {
        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(Regex.Replace(str,
                @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
        }
    }
}
