using OData.QueryBuilder.Conventions.Constants;
using System;
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

        public static IEnumerable<string> ReplaceWithStringBuilder(this IEnumerable<string> values, IDictionary<string, string> keyValuePairs)
        {
            var replaceValues = new List<string>();

            foreach (var value in values)
            {
                replaceValues.Add(value.ReplaceWithStringBuilder(keyValuePairs));
            }

            return replaceValues;
        }

        public static string replaceSpecialCharacters(string attribute)
        {
            attribute = attribute.Replace("'", "''");
            attribute = attribute.Replace("+", "%2B");
            attribute = attribute.Replace("/", "%2F");
            attribute = attribute.Replace("?", "%3F");
            attribute = attribute.Replace("%", "%25");
            attribute = attribute.Replace("#", "%23");
            attribute = attribute.Replace("&", "%26");

            return attribute;
        }
        
        public static string ToQuery(this object @object)
        {
            switch (@object)
            {
                case null:
                    return "null";
                case string @string:
                    return $"'{replaceSpecialCharacters(@string)}'";
                case bool @bool:
                    return $"{@bool}".ToLowerInvariant();
                case DateTime dateTime:
                    return $"{dateTime:s}Z";
                case DateTimeOffset dateTimeOffset:
                    return $"{dateTimeOffset:s}Z";
                case IEnumerable<int> intValues:
                    var intValuesString = string.Join(QuerySeparators.Comma, intValues);

                    return !string.IsNullOrEmpty(intValuesString) ? intValuesString : string.Empty;
                case IEnumerable<string> stringValues:
                    var stringValuesString = string.Join($"'{QuerySeparators.Comma}'", stringValues);

                    return !string.IsNullOrEmpty(stringValuesString) ? $"'{stringValuesString}'" : string.Empty;
                case Guid @guid:
                    return $"{@guid}";
                default:
                    return @object.GetType().IsPrimitive ?
                        Convert.ToString(@object, CultureInfo.InvariantCulture) : $"'{@object}'";
            }
        }
    }
}
