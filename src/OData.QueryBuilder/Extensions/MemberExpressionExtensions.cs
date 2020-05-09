using System.Linq.Expressions;

namespace OData.QueryBuilder.Extensions
{
    internal static class MemberExpressionExtensions
    {
        public static object GetMemberValue(this MemberExpression memberExpression)
        {
            if (memberExpression.Expression is ConstantExpression ce)
            {
                return memberExpression.Member.GetValue(ce.Value);
            }

            if (memberExpression.Expression is MemberExpression me)
            {
                return memberExpression.Member.GetValue(GetMemberValue(me));
            }

            return memberExpression.Member.GetValue();
        }

        public static string ToODataQuery(this MemberExpression memberExpression, string queryString)
        {
            var memberExpressionValue = memberExpression.GetMemberValue();

            if (memberExpressionValue != default(object))
            {
                if (memberExpressionValue is string)
                {
                    return $"'{memberExpressionValue}'";
                }

                if (memberExpressionValue is bool)
                {
                    return $"{memberExpressionValue}".ToLower();
                }

                return $"{memberExpressionValue}";
            }

            var parentMemberExpressionQuery = memberExpression.Expression.ToODataQuery(queryString);

            if (string.IsNullOrEmpty(parentMemberExpressionQuery))
            {
                return memberExpression.Member.Name;
            }

            return $"{parentMemberExpressionQuery}/{memberExpression.Member.Name}";
        }
    }
}
