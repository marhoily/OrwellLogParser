using System;

namespace LogParser.Utilities
{
    public static class StringExtensions
    {
        public static string RemoveAllBefore(this string x, string substring)
        {
            var idx = x.IndexOf(substring, StringComparison.Ordinal);
            return idx == -1 ? x : x.Substring(idx);
        }
        public static string FromSubstringTillTheEndoOfLine(this string x, string substring)
        {
            var strings = x.Split(new [] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in strings)
            {
                var idx = s.IndexOf(substring, StringComparison.Ordinal);
                if (idx != -1) return s.Substring(idx);
            }

            return x;
        }
    }
}