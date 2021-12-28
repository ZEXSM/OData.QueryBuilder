using OData.QueryBuilder.Conventions.Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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

        public static IEnumerable<string> ReplaceWithStringBuilder(this ICollection<string> values, IDictionary<string, string> keyValuePairs)
        {
            var index = 0;
            var replaceValues = new string[values.Count];

            foreach (var value in values)
            {
                replaceValues[index] = value.ReplaceWithStringBuilder(keyValuePairs);
                index++;
            }

            return replaceValues;
        }

        public static string ToQuery(this object @object)
        {
            return @object switch
            {
                null => "null",
                bool @bool => $"{@bool}".ToLowerInvariant(),
                DateTime dateTime => $"{dateTime:s}Z",
                DateTimeOffset dateTimeOffset => $"{dateTimeOffset:s}Z",
                ICollection collection => collection.ToQuery(),
                Guid @guid => $"{@guid}",
                _ => @object.GetType().IsPrimitive ? Convert.ToString(@object, CultureInfo.InvariantCulture) : $"'{@object}'",
            };
        }

        private static string ToQuery(this ICollection collection)
        {
            var index = 0;
            var queries = new string[collection.Count];

            foreach (var item in collection)
            {
                queries[index] = item.ToQuery();
                index++;
            }

            return string.Join(QuerySeparators.Comma, queries) ?? string.Empty;
        }
    }
}
