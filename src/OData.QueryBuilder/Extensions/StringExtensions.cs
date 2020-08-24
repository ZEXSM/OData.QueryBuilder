using System;
using System.Collections.Generic;
using System.Text;

namespace OData.QueryBuilder.Extensions
{
    internal static class StringExtensions
    {
        public static bool IsNullOrQuotes(this string value) =>
            string.IsNullOrEmpty(value) || value == "null" || value == "''";

        public static string ReplaceWithStringBuilder(this string value, IEnumerable<Tuple<string, string>> tuples)
        {
            var stringBuilder = new StringBuilder(value);

            foreach (var (oldChar, newChar) in tuples)
            {
                stringBuilder.Replace(oldChar, newChar);
            }

            return stringBuilder.ToString();
        }
    }
}
