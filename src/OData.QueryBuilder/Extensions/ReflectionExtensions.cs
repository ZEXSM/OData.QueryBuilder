using System;
using System.Reflection;

namespace OData.QueryBuilder.Extensions
{
    internal static class ReflectionExtensions
    {
        public static object GetValue(this MemberInfo memberInfo, object obj = default)
        {
            try
            {
                return memberInfo switch
                {
                    FieldInfo fieldInfo => fieldInfo.GetValue(obj),
                    PropertyInfo propertyInfo => propertyInfo.GetValue(obj, default),
                    _ => default,
                };
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static bool IsNullableType(this Type type) =>
            Nullable.GetUnderlyingType(type) != default;
    }
}
