using OData.QueryBuilder.Conventions.Constants;
using OData.QueryBuilder.Options;
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

        public static string ToValue(this object @object, ODataQueryBuilderOptions options)
        {
            switch (options.Mode)
            {
                case ODataQueryBuilderMode.TemplateUri:
                    return @object switch
                    {
                        null => options.TemplateKeyValue,
                        bool _ => options.TemplateKeyValue,
                        DateTime _ => options.TemplateKeyValue,
                        DateTimeOffset _ => options.TemplateKeyValue,
                        string _ => $"'{options.TemplateKeyValue}'",
                        ICollection _ => options.TemplateKeyValue,
                        IEnumerable _ => options.TemplateKeyValue,
                        Guid _ => options.TemplateKeyValue,
                        decimal _ => options.TemplateKeyValue,
                        _ => @object.GetType().IsPrimitive ? options.TemplateKeyValue : $"'{options.TemplateKeyValue}'",
                    };
                case ODataQueryBuilderMode.Uri:
                default:
                    return @object switch
                    {
                        null => Null,
                        bool @bool => @bool ? "true" : "false",
                        DateTime dateTime => options switch
                        {
                            { UseCorrectDateTimeFormat: true } =>
                                $"{dateTime:yyyy-MM-ddTHH:mm:sszzz}".Replace("+", "%2B"),
                            _ => $"{dateTime:s}Z"
                        },
                        DateTimeOffset dateTimeOffset => options switch
                        {
                            { UseCorrectDateTimeFormat: true } =>
                                $"{dateTimeOffset:yyyy-MM-ddTHH:mm:sszzz}".Replace("+", "%2B"),
                            _ => $"{dateTimeOffset:s}Z"
                        },
                        string @string => $"'{@string}'",
                        ICollection collection => collection.CollectionToQuery(options),
                        IEnumerable enumerable => enumerable.EnumerableToQuery(options, initCount: 0),
                        Guid @guid => $"{@guid}",
                        decimal @decimal => Convert.ToString(@decimal, CultureInfo.InvariantCulture),
                        _ => @object.GetType().IsPrimitive ? Convert.ToString(@object, CultureInfo.InvariantCulture) : $"'{@object}'",
                    };
            }
        }

        private static string CollectionToQuery(
            this ICollection collection,
            ODataQueryBuilderOptions options) =>
            collection.EnumerableToQuery(options, initCount: collection.Count);

        private static string EnumerableToQuery(
            this IEnumerable enumerable,
            ODataQueryBuilderOptions options,
            int initCount)
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
                queries[index] = item.ToValue(options);
                index++;
            }

            return string.Join(QuerySeparators.StringComma, queries) ?? string.Empty;
        }
    }
}
