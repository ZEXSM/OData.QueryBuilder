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
            switch (@object)
            {
                case null:
                    return "null";
                case string @string:
                    return $"'{@string}'";
                case bool @bool:
                    return $"{@bool}".ToLower();
                case int @int:
                    return $"{@int}";
                case DateTime dateTime:
                    return $"{dateTime:s}Z";
                case DateTimeOffset dateTimeOffset:
                    return $"{dateTimeOffset:s}Z";
                case IEnumerable<int> intValues:
                    var intValuesString = string.Join(QuerySeparators.CommaString, intValues);

                    return !string.IsNullOrEmpty(intValuesString) ? intValuesString : string.Empty;
                case IEnumerable<string> stringValues:
                    var stringValuesString = string.Join($"'{QuerySeparators.CommaString}'", stringValues);

                    return !string.IsNullOrEmpty(stringValuesString) ? $"'{stringValuesString}'" : string.Empty;
                default:
                    return $"{@object}";
            }
        }
    }
}
