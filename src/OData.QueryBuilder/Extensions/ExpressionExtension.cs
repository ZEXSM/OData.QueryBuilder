using System.Linq.Expressions;

namespace OData.QueryBuilder.Extensions
{
    internal static class ExpressionExtension
    {
        public static string ToODataOperator(this ExpressionType expressionType)
        {
            switch (expressionType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return "and";
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return "or";
                case ExpressionType.Equal:
                    return "eq";
                case ExpressionType.NotEqual:
                    return "ne";
                case ExpressionType.LessThan:
                    return "lt";
                case ExpressionType.LessThanOrEqual:
                    return "le";
                case ExpressionType.GreaterThan:
                    return "gt";
                case ExpressionType.GreaterThanOrEqual:
                    return "ge";
                default:
                    return string.Empty;
            }
        }

        public static string ToODataQuery(this UnaryExpression unaryExpression) =>
            ((MemberExpression)unaryExpression.Operand).Member.Name;

        public static string ToODataQuery(this MemberExpression memberExpression) =>
            memberExpression.Member.Name;

        public static string ToODataQuery(this NewExpression newExpression)
        {
            var names = new string[newExpression.Members.Count];

            for (var i = 0; i < newExpression.Members.Count; i++)
            {
                names[i] = newExpression.Members[i].Name;
            }

            return string.Join(",", names);
        }
    }
}
