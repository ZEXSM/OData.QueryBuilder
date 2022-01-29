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
        private const string Null = "null";

        public static bool IsNullOrQuotes(this string value) =>
            string.IsNullOrEmpty(value) || value == Null || value == "''";

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

        public static string ToQuery(this object @object) => @object switch
        {
            null => Null,
            bool @bool => @bool ? "true" : "false",
            DateTime dateTime => $"{dateTime:s}Z",
            DateTimeOffset dateTimeOffset => $"{dateTimeOffset:s}Z",
            string @string => $"'{@string}'",
            ICollection collection => collection.CollectionToQuery(),
            IEnumerable enumerable => enumerable.EnumerableToQuery(),
            Guid @guid => $"{@guid}",
            _ => @object.GetType().IsPrimitive ? Convert.ToString(@object, CultureInfo.InvariantCulture) : $"'{@object}'",
        };

        private static string CollectionToQuery(this ICollection collection) =>
            collection.EnumerableToQuery(collection.Count);

        private static string EnumerableToQuery(this IEnumerable enumerable, int initCount = 0)
        {
            var index = 0;
            var count = 0;

            if (initCount <= 0)
            {
                foreach (var _ in enumerable)
                {
                    count++;
                }
            }
            else
            {
                count = initCount;
            }

            var queries = new string[count];

            foreach (var item in enumerable)
            {
                queries[index] = item.ToQuery();
                index++;
            }

            return string.Join(QuerySeparators.Comma, queries) ?? string.Empty;
        }
    }
}
