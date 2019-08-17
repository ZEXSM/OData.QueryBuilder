using System;
using System.Reflection;

namespace OData.QueryBuilder.Extensions
{
    internal static class ReflectionExtension
    {
        public static object GetValue(this MemberInfo memberInfo, object obj = default(object))
        {
            try
            {
                switch (memberInfo)
                {
                    case FieldInfo fieldInfo:
                        return fieldInfo.GetValue(obj);
                    case PropertyInfo propertyInfo:
                        return propertyInfo.GetValue(obj, default(object[]));
                    default:
                        return default(object);
                }
            }
            catch (Exception)
            {
                return default(object);
            }
        }
    }
}
