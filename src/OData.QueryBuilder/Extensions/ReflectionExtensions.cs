using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace OData.QueryBuilder.Extensions
{
    [ExcludeFromCodeCoverage]
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
    }
}
