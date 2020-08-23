using OData.QueryBuilder.Conventions.Constants;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OData.QueryBuilder.Extensions
{
    internal static class ReflectionExtensions
    {
        public static object GetValue(this MemberInfo memberInfo, object obj = default) => memberInfo switch
        {
            FieldInfo fieldInfo => fieldInfo.GetValue(obj),
            PropertyInfo propertyInfo => propertyInfo.GetValue(obj, default),
            _ => default,
        };

        public static bool IsNullableType(this Type type) =>
            Nullable.GetUnderlyingType(type) != default;

        public static string ConvertToString(object @object)
        {
            string toReturn;

            switch (@object)
            {
                case null:
                    return "null";
                case string @string:
                    toReturn = $"'{@string}'";
                    break;
                case bool @bool:
                    toReturn = $"{@bool}".ToLower();
                    break;
                case int @int:
                    toReturn = $"{@int}";
                    break;
                case DateTime dateTime:
                    toReturn = $"{dateTime:s}Z";
                    break;
                case DateTimeOffset dateTimeOffset:
                    toReturn = $"{dateTimeOffset:s}Z";
                    break;
                case IEnumerable<int> intValues:
                    var intValuesString = string.Join(QuerySeparators.CommaString, intValues);

                    toReturn = !string.IsNullOrEmpty(intValuesString) ? intValuesString : string.Empty;
                    break;
                case IEnumerable<string> stringValues:
                    var stringValuesString = string.Join($"'{QuerySeparators.CommaString}'", stringValues);

                    toReturn = !string.IsNullOrEmpty(stringValuesString) ? $"'{stringValuesString}'" : string.Empty;
                    break;
                default:
                    return $"{@object}";
            }

            return toReturn.Replace("&", "%26");
        }
    }
}
