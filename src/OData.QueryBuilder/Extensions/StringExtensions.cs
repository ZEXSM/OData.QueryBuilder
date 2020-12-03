using System.Collections.Generic;
using System.Text;

namespace OData.QueryBuilder.Extensions
{
    internal static class StringExtensions
    {
        public static bool IsNullOrQuotes(this string value) =>
            string.IsNullOrEmpty(value) || value == "null" || value == "''";

        public static string ReplaceWithStringBuilder(this string value, IDictionary<string, string> keyValuePairs)
        {
            var stringBuilder = new StringBuilder(value);

            foreach (var keyValuePair in keyValuePairs)
            {
                stringBuilder.Replace(keyValuePair.Key, keyValuePair.Value);
            }

            return stringBuilder.ToString();
        }

        public static IEnumerable<string> ReplaceWithStringBuilder(this IEnumerable<string> values, IDictionary<string, string> keyValuePairs)
        {
            var replaceValues = new List<string>();

            foreach (var value in values)
            {
                replaceValues.Add(value.ReplaceWithStringBuilder(keyValuePairs));
            }

            return replaceValues;
        }
    }
}
