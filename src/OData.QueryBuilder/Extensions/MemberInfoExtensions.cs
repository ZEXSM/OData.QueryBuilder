using System;
using System.Reflection;

namespace OData.QueryBuilder.Extensions
{
    internal static class MemberInfoExtensions
    {
        public static object GetValue(this MemberInfo memberInfo, object obj = default)
        {
            try
            {
                switch (memberInfo)
                {
                    case FieldInfo fieldInfo:
                        return fieldInfo.GetValue(obj);
                    case PropertyInfo propertyInfo:
                        return propertyInfo.GetValue(obj, default);
                    default:
                        return default;
                }
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
