using System;
using System.Linq.Expressions;

namespace OData.QueryBuilder.Extensions
{
    internal static class MemberExpressionExtensions
    {
        public static string ToODataQuery(this MemberExpression memberExpression, string queryString)
        {
            var memberExpressionValue = memberExpression.GetValue();

            if (memberExpressionValue != default)
            {
                if (memberExpressionValue is string @string)
                {
                    return $"'{@string}'";
                }

                if (memberExpressionValue is bool @bool)
                {
                    return $"{@bool}".ToLower();
                }

                if (memberExpressionValue is DateTime dateTime)
                {
                    return $"{dateTime:s}Z";
                }

                if (memberExpressionValue is DateTimeOffset dateTimeOffset)
                {
                    return $"{dateTimeOffset:s}Z";
                }

                return $"{memberExpressionValue}";
            }

            var parentMemberExpressionQuery = memberExpression.Expression.ToODataQuery(queryString);

            if (string.IsNullOrEmpty(parentMemberExpressionQuery))
            {
                return memberExpression.Member.Name;
            }

            return memberExpression.Member.DeclaringType.IsNullableType() ?
                parentMemberExpressionQuery
                :
                $"{parentMemberExpressionQuery}/{memberExpression.Member.Name}"
                ;
        }

        public static object GetValue(this MemberExpression memberExpression)
        {
            if (memberExpression.Expression is ConstantExpression ce)
            {
                return memberExpression.Member.GetValue(ce.Value);
            }

            if (memberExpression.Expression is MemberExpression me)
            {
                return memberExpression.Member.GetValue(GetValue(me));
            }

            return memberExpression.Member.GetValue();
        }
    }
}
